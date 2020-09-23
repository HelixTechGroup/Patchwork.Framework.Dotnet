using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;

namespace Patchwork.Framework
{
    public abstract class PatchworkActivity<TView> : Activity where TView : View
    {
        protected TView m_view;
        protected INWindow m_window;

        public TView View { get { return m_view; } }

        /// <inheritdoc />
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            CreateActivity();
        }

        /// <inheritdoc />
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState, persistentState);

            CreateActivity();
        }

        protected virtual void CreateActivity()
        {
            m_view = Activator.CreateInstance(typeof(TView), this) as TView;
            SetContentView(m_view);
            m_window = Core.Application.CurrentWindow;
            Title = m_window.Title;
            Core.MessagePump.PushWindowMessage(WindowMessageIds.Created, m_window);
        }

        /// <inheritdoc />
        protected override void OnStart()
        {
            base.OnStart();
            m_window = Core.Application.CurrentWindow;
            m_window.Initialize();            
        }

        /// <inheritdoc />
        protected override void OnPause()
        {
            Core.MessagePump.PushWindowMessage(WindowMessageIds.Deactivated, m_window);
            base.OnPause();
        }

        /// <inheritdoc />
        protected override void OnResume()
        {
            Core.MessagePump.PushWindowMessage(WindowMessageIds.Activating, m_window);
            base.OnResume();
        }

        /// <inheritdoc />
        protected override void OnPostResume()
        {
            Core.MessagePump.PushWindowMessage(WindowMessageIds.Activated, m_window);
            base.OnPostResume();
        }

        /// <inheritdoc />
        protected override void OnStop()
        {
            Core.MessagePump.PushWindowMessage(WindowMessageIds.Closed, m_window);
            base.OnStop();
        }

        /// <inheritdoc />
        protected override void OnDestroy()
        {
            Core.MessagePump.PushWindowMessage(WindowMessageIds.Destroyed, m_window);
            base.OnDestroy();
        }

        internal void Destroy()
        {
            OnDestroy();
        }        
    }

    [Activity]
    public class PatchworkActivity : PatchworkActivity<FrameLayout> 
    {
    }
}
