#region Usings
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace Patchwork.Framework.Services.Threading
{
    //public sealed class WindowsPlatformUiDispatcher : ThreadDispatcher
    //{
    //    #region Methods
    //    public override void Run(Action action, Action callback = null, CancellationToken cancellationToken = default)
    //    {
    //        if (CheckAccess())
    //        {
    //            cancellationToken.ThrowIfCancellationRequested();
    //            action();

    //            if (callback != null)
    //            {
    //                cancellationToken.ThrowIfCancellationRequested();
    //                callback();
    //            }
    //        }
    //        else
    //        {
    //            Exception exception = null;
    //            Action method = () =>
    //                            {
    //                                try
    //                                {
    //                                    cancellationToken.ThrowIfCancellationRequested();
    //                                    action();

    //                                    if (callback != null)
    //                                    {
    //                                        cancellationToken.ThrowIfCancellationRequested();
    //                                        callback();
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    exception = ex;
    //                                }
    //                            };
    //            //Dispatcher.CurrentDispatcher.Invoke(method, DispatcherPriority.Normal, cancellationToken);
    //            if (exception != null)
    //                throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
    //        }
    //    }

    //    public override Task RunAsync(Action action, Action<Task> callback = null, CancellationToken cancellationToken = default)
    //    {
    //        VerifyDispatcher();
    //        var task = new Task(() => { });//Dispatcher.CurrentDispatcher.InvokeAsync(action, DispatcherPriority.Normal, cancellationToken).Task;
    //        if (callback != null)
    //            task.ContinueWith(callback, cancellationToken);

    //        return task;
    //    }

    //    public override void Run<T>(Action<T> action,
    //                                T parameter,
    //                                Action callback = null,
    //                                CancellationToken cancellationToken = default)
    //    {
    //        if (CheckAccess())
    //        {
    //            cancellationToken.ThrowIfCancellationRequested();
    //            action(parameter);
    //            if (callback != null)
    //            {
    //                cancellationToken.ThrowIfCancellationRequested();
    //                callback();
    //            }
    //        }
    //        else
    //        {
    //            Exception exception = null;
    //            Action method = () =>
    //                            {
    //                                try
    //                                {
    //                                    action(parameter);

    //                                    if (callback != null)
    //                                    {
    //                                        cancellationToken.ThrowIfCancellationRequested();
    //                                        callback();
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    exception = ex;
    //                                }
    //                            };
    //            //Dispatcher.CurrentDispatcher.Invoke(method);
    //            if (exception != null)
    //                throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
    //        }
    //    }

    //    public override Task RunAsync<T>(Action<T> action,
    //                                     T parameter,
    //                                     Action<Task> callback = null,
    //                                     CancellationToken cancellationToken = default)
    //    {
    //        VerifyDispatcher();
    //        var taskSource = new TaskCompletionSource<object>();
    //        Action method = () =>
    //                        {
    //                            try
    //                            {
    //                                action(parameter);
    //                                taskSource.SetResult(null);
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                taskSource.SetException(ex);
    //                            }
    //                        };
    //        //Dispatcher.CurrentDispatcher.BeginInvoke(method);
    //        var task = taskSource.Task;
    //        if (callback != null)
    //            task.ContinueWith(callback, cancellationToken);
    //        return task;
    //    }

    //    protected override void VerifyDispatcher()
    //    {
    //        if (!CheckAccess())
    //            throw new InvalidOperationException("Not initialized with dispatcher.");
    //    }

    //    protected override bool CheckAccess()
    //    {
    //        return Dispatcher.CurrentDispatcher.CheckAccess();
    //    }
    //    #endregion
    //}
}