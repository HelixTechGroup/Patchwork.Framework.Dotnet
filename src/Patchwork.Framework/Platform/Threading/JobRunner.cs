// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patchwork.Framework.Manager;
#endregion

namespace Patchwork.Framework.Platform.Threading
{
    /// <summary>
    /// A main loop in a <see cref="Dispatcher"/>.
    /// </summary>
    internal class JobRunner
    {
        #region Members
        private readonly IList<Queue<IJob>> _queues = Enumerable.Range(0, (int)NThreadDispatcherPriority.MaxValue + 1)
                                                           .Select(_ => new Queue<IJob>()).ToArray();

        private INThreadDispatcher _platform;
        #endregion

        public bool CheckAccess()
        {
            return _platform?.CurrentThreadIsLoopThread ?? true;
        }

        public JobRunner(INThreadDispatcher platform)
        {
            _platform = platform;
        }

        #region Methods
        /// <summary>
        /// Runs continuations pushed on the loop.
        /// </summary>
        /// <param name="priority">Priority to execute jobs for. Pass null if platform doesn't have internal priority system</param>
        public void RunJobs(NThreadDispatcherPriority? priority)
        {
            var minimumPriority = priority ?? NThreadDispatcherPriority.MinValue;
            while (true)
            {
                var job = GetNextJob(minimumPriority);
                if (job == null)
                    return;

                job.Run();
            }
        }

        /// <summary>
        /// Invokes a method on the main loop.
        /// </summary>
        /// <param name="action">The method.</param>
        /// <param name="priority">The priority with which to invoke the method.</param>
        /// <returns>A task that can be used to track the method's execution.</returns>
        public Task InvokeAsync(Action action, NThreadDispatcherPriority priority)
        {
            var job = new Job(action, priority, false);
            AddJob(job);
            return job.Task;
        }

        /// <summary>
        /// Invokes a method on the main loop.
        /// </summary>
        /// <param name="function">The method.</param>
        /// <param name="priority">The priority with which to invoke the method.</param>
        /// <returns>A task that can be used to track the method's execution.</returns>
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, NThreadDispatcherPriority priority)
        {
            var job = new Job<TResult>(function, priority);
            AddJob(job);
            return job.Task;
        }

        /// <summary>
        /// Post action that will be invoked on main thread
        /// </summary>
        /// <param name="action">The method.</param>
        /// 
        /// <param name="priority">The priority with which to invoke the method.</param>
        internal void Post(Action action, NThreadDispatcherPriority priority)
        {
            AddJob(new Job(action, priority, true));
        }

        /// <summary>
        /// Allows unit tests to change the platform threading interface.
        /// </summary>
        internal void UpdateServices()
        {
            _platform = Core.Dispatcher;//Shield.CurrentApplication.Container.Resolve<INativeThreadDispatcher>();
        }

        private void AddJob(IJob job)
        {
            bool needWake;
            var queue = _queues[(int)job.Priority];
            lock(queue)
            {
                needWake = queue.Count == 0;
                queue.Enqueue(job);
            }

            if (needWake)
                _platform?.Signal(job.Priority);
        }

        private IJob GetNextJob(NThreadDispatcherPriority minimumPriority)
        {
            for (int c = (int)NThreadDispatcherPriority.MaxValue; c >= (int)minimumPriority; c--)
            {
                var q = _queues[c];
                lock(q)
                {
                    //if (q.Count > 0)
                    //    return q.Dequeue();
                    q.TryDequeue(out var job);
                    return job;
                }
            }

            return null;
        }
        #endregion

        #region Nested Types
        private interface IJob
        {
            #region Properties
            /// <summary>
            /// Gets the job priority.
            /// </summary>
            NThreadDispatcherPriority Priority { get; }
            #endregion

            #region Methods
            /// <summary>
            /// Runs the job.
            /// </summary>
            void Run();
            #endregion
        }

        /// <summary>
        /// A job to run.
        /// </summary>
        private sealed class Job : IJob
        {
            #region Members
            /// <summary>
            /// The method to call.
            /// </summary>
            private readonly Action _action;

            /// <summary>
            /// The task completion source.
            /// </summary>
            private readonly TaskCompletionSource<object> _taskCompletionSource;
            #endregion

            #region Properties
            /// <inheritdoc/>
            public NThreadDispatcherPriority Priority { get; }

            /// <summary>
            /// The task.
            /// </summary>
            public Task Task
            {
                get { return _taskCompletionSource?.Task; }
            }
            #endregion

            /// <summary>
            /// Initializes a new instance of the <see cref="Job"/> class.
            /// </summary>
            /// <param name="action">The method to call.</param>
            /// <param name="priority">The job priority.</param>
            /// <param name="throwOnUiThread">Do not wrap exception in TaskCompletionSource</param>
            public Job(Action action, NThreadDispatcherPriority priority, bool throwOnUiThread)
            {
                _action = action;
                Priority = priority;
                _taskCompletionSource = throwOnUiThread ? null : new TaskCompletionSource<object>();
            }

            #region Methods
            /// <inheritdoc/>
            void IJob.Run()
            {
                if (_taskCompletionSource == null)
                {
                    _action();
                    return;
                }

                try
                {
                    _action();
                    _taskCompletionSource.SetResult(null);
                }
                catch (Exception e)
                {
                    _taskCompletionSource.SetException(e);
                }
            }
            #endregion
        }

        /// <summary>
        /// A job to run.
        /// </summary>
        private sealed class Job<TResult> : IJob
        {
            #region Members
            private readonly Func<TResult> _function;
            private readonly TaskCompletionSource<TResult> _taskCompletionSource;
            #endregion

            #region Properties
            /// <inheritdoc/>
            public NThreadDispatcherPriority Priority { get; }

            /// <summary>
            /// The task.
            /// </summary>
            public Task<TResult> Task
            {
                get { return _taskCompletionSource.Task; }
            }
            #endregion

            /// <summary>
            /// Initializes a new instance of the <see cref="Job"/> class.
            /// </summary>
            /// <param name="function">The method to call.</param>
            /// <param name="priority">The job priority.</param>
            public Job(Func<TResult> function, NThreadDispatcherPriority priority)
            {
                _function = function;
                Priority = priority;
                _taskCompletionSource = new TaskCompletionSource<TResult>();
            }

            #region Methods
            /// <inheritdoc/>
            void IJob.Run()
            {
                try
                {
                    var result = _function();
                    _taskCompletionSource.SetResult(result);
                }
                catch (Exception e)
                {
                    _taskCompletionSource.SetException(e);
                }
            }
            #endregion
        }
        #endregion
    }
}