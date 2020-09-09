using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Java.Interop;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;

namespace Patchwork.Framework.Platform
{
    public class AndroidApplication : NativeApplication
    {
        private static ActivityStatusTracker m_tracker;

        public Activity CurrentActivity
        {
            get { return m_tracker.Current; }
        }

        public Activity PreviousActivity
        {
            get { return m_tracker.Previous; }
        }

        public Context Context
        {
            get { return Application.Context; }
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            var app = (Context as Application);
            m_handle = new NativeHandle(app?.Handle ?? IntPtr.Zero, "");
            m_tracker = new ActivityStatusTracker();
            app?.RegisterActivityLifecycleCallbacks(m_tracker);
            m_tracker.WaitForActivityAsync().ConfigureAwait(false);
            base.InitializeResources();
        }

        protected override INativeWindow PlatformCreateWindow()
        {
            var def = new NativeWindowDefinition()
                      {
                          AcceptsInput = true,
                          DesiredSize = new Size(800, 600),
                          IsRegularWindow = true,
                          Title = "test",
                          Type = NativeWindowType.Normal,
                          IsMainApplicationWindow = true
                      };
            var win = new AndroidWindow(this, def);
            win.Create(false);
            m_mainWindow = m_currentWindow = win;

            return win;
        }

        /// <inheritdoc />
        protected override void PlatformPumpMessages(CancellationToken cancellationToken)
        {
            
        }
    }
}
