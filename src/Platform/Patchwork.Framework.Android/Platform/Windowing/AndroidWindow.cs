#region Usings
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Util;
using Android.Views;
using Java.Lang;
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework.ComponentModel;
using NWindow = Patchwork.Framework.Platform.Windowing.NWindow;
using Size = System.Drawing.Size;
#endregion

namespace Patchwork.Framework.Platform.Windowing
{
    public sealed partial class AndroidWindow : NWindow, IEquatable<AndroidWindow>
    {
        #region Members
        private Activity m_activity;
        private PatchworkViewListener m_viewListener;
        private PatchworkViewTreeObserverListener m_viewTreeObserver;
        #endregion

        public AndroidWindow(INObject parent, NWindowDefinition definition) : base(parent, definition) { }

        #region Methods
        /// <inheritdoc />
        protected override void InitializeResources()
        {
            m_activity = (m_parent as AndroidApplication)?.CurrentActivity;
            m_viewTreeObserver = new PatchworkViewTreeObserverListener(this, m_activity?.Window.DecorView);
            m_viewListener = new PatchworkViewListener(this, m_activity?.Window.DecorView);
            m_handle = new NHandle(m_activity?.Handle ?? IntPtr.Zero, "");
            base.InitializeResources();
        }

        /// <inheritdoc />
        protected override bool PlatformCheckActive()
        {
            return m_activity?.Window?.IsActive ?? false;
        }

        /// <inheritdoc />
        protected override bool PlatformCheckEnable()
        {
            return true;
        }

        /// <inheritdoc />
        protected override bool PlatformCheckFocus()
        {
            return m_activity?.HasWindowFocus ?? false;
        }

        /// <inheritdoc />
        protected override bool PlatformCheckVisible()
        {
            var vis = m_activity?.Window?.DecorView.Visibility;
            return vis == ViewStates.Visible;
        }

        /// <inheritdoc />        
        protected override void PlatformCreate()
        {
            var parent = m_parent as INApplication;
            var intent = new Intent(Application.Context, Class.FromType(typeof(PatchworkActivity)));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }

        /// <inheritdoc />
        protected override void PlatformDestroy()
        {
            (m_activity as PatchworkActivity)?.Destroy();
        }

        /// <inheritdoc />
        protected override Rectangle PlatformGetClientArea()
        {
            var size = PlatformGetClientSize();
            return new Rectangle(0, 0, size.Width, size.Height);
        }

        /// <inheritdoc />
        protected override Size PlatformGetClientSize()
        {
            var displayMetrics = new DisplayMetrics();
            m_activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            return new Size(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
        }

        /// <inheritdoc />
        protected override Size PlatformGetMaxClientSize()
        {
            return PlatformGetWindowSize();
        }

        /// <inheritdoc />
        protected override Point PlatformGetPosition()
        {
            var pos = Point.Empty;
            var view = m_activity?.Window?.DecorView;
            view?.Post(() =>
                      {
                          var point = new int[2];
                          view.GetLocationOnScreen(point);
                          pos = new Point(point[0], point[1]);
                      });

            return pos;
        }

        /// <inheritdoc />
        protected override string PlatformGetTitle()
        {
            return m_activity.Title;
        }

        /// <inheritdoc />
        protected override Size PlatformGetWindowSize()
        {
            var displayMetrics = new DisplayMetrics();
            m_activity.WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            return new Size(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
        }

        /// <inheritdoc />
        protected override Point PlatformPointToClient(Point point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Point PlatformPointToScreen(Point point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void PlatformSetTitle(string value)
        {
            if (Application.Context.MainLooper is null || m_activity.ActionBar is null) 
                return;

            using var handle = new Handler(Application.Context.MainLooper);
            handle.Post(() =>
                        {
                            m_activity.Title = value;
                            m_activity.ActionBar.Title = value;
                        });
        }

        /// <inheritdoc />
        protected override void PlatformSyncDataCache()
        {
            m_cache.ClientSize = PlatformGetClientSize();
            m_cache.MaxClientSize = PlatformGetMaxClientSize();
            m_cache.ClientArea = PlatformGetClientArea();
            m_cache.IsActive = PlatformCheckActive();
            m_cache.IsEnabled = PlatformCheckEnable();
            m_cache.IsFocused = PlatformCheckFocus();
            m_cache.IsVisible = PlatformCheckVisible();
            m_cache.Position = PlatformGetPosition();
            m_cache.Size = PlatformGetWindowSize();
        }

        /// <inheritdoc />
        protected override void OnInitialized(object sender, EventArgs e)
        {
            SetWindowTitle(m_cache.Title);
            base.OnInitialized(sender, e);
        }
        #endregion

        /// <inheritdoc />
        public bool Equals(AndroidWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(m_activity, other.m_activity);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(AndroidWindow)) return false;
            return Equals((AndroidWindow)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), m_activity);
        }

        public static bool operator ==(AndroidWindow left, AndroidWindow right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AndroidWindow left, AndroidWindow right)
        {
            return !Equals(left, right);
        }
    }
}