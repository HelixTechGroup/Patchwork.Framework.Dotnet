// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

#region Usings
using System.Threading;
using Patchwork.Framework;
using Patchwork.Framework.Platform;
using Patchwork.Framework.Platform.Threading;
#endregion

namespace Shield.Framework.Threading
{
    /// <summary>
    /// SynchronizationContext to be used on main thread
    /// </summary>
    public class NThreadDispatcherSynchronizationContext : SynchronizationContext
    {
        private static JobRunner m_jobRunner;

        #region Properties
        /// <summary>
        /// Controls if SynchronizationContext should be installed in InstallIfNeeded. Used by Designer.
        /// </summary>
        public static bool AutoInstall { get; set; } = true;
        #endregion

        #region Methods
        public NThreadDispatcherSynchronizationContext()
        {
            if (m_jobRunner != null) 
                return;

            m_jobRunner = new JobRunner(Core.Dispatcher);
            Core.Dispatcher.Signaled += m_jobRunner.RunJobs;
        }

        /// <summary>
        /// Installs synchronization context in current thread
        /// </summary>
        public static void InstallIfNeeded()
        {
            if (!AutoInstall || Current is NThreadDispatcherSynchronizationContext) return;

            SetSynchronizationContext(new NThreadDispatcherSynchronizationContext());
        }

        /// <inheritdoc/>
        public override void Post(SendOrPostCallback d, object state)
        {
            m_jobRunner.InvokeAsync(() => d(state), NThreadDispatcherPriority.Send);
        }

        /// <inheritdoc/>
        public override void Send(SendOrPostCallback d, object state)
        {
            if (m_jobRunner.CheckAccess())
                d(state);
            else
            {
                var t = m_jobRunner.InvokeAsync(() => d(state), NThreadDispatcherPriority.Send).ConfigureAwait(false);
            }

        }
        #endregion
    }
}