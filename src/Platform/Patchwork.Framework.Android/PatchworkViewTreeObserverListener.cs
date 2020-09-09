using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Android.Runtime;
using Android.Views;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform
{
    public sealed partial class AndroidWindow
    {
        [Preserve(AllMembers = true)]
        protected class PatchworkViewTreeObserverListener : Java.Lang.Object,
                                               ViewTreeObserver.IOnDrawListener,
                                               ViewTreeObserver.IOnWindowFocusChangeListener,
                                               ViewTreeObserver.IOnGlobalFocusChangeListener,
                                               ViewTreeObserver.IOnGlobalLayoutListener,
                                               ViewTreeObserver.IOnPreDrawListener
        {
            private readonly View m_view;
            private readonly INativeWindow m_window;

            public PatchworkViewTreeObserverListener(INativeWindow window, View view)
            {
                m_view = view;
                m_window = window;
                m_view.ViewTreeObserver.AddOnDrawListener(this);
                m_view.ViewTreeObserver.AddOnGlobalFocusChangeListener(this);
                m_view.ViewTreeObserver.AddOnGlobalLayoutListener(this);
                m_view.ViewTreeObserver.AddOnWindowFocusChangeListener(this);
            }

            /// <inheritdoc />
            public void OnDraw()
            {

            }

            /// <inheritdoc />
            public void OnGlobalFocusChanged(View oldFocus, View newFocus)
            {

            }

            /// <inheritdoc />
            public void OnGlobalLayout()
            {
            }

            /// <inheritdoc />
            public void OnWindowFocusChanged(bool hasFocus)
            {
                Core.MessagePump.PushWindowFocusChangingMessage(m_window, !hasFocus, hasFocus);
                if (hasFocus)
                    Core.MessagePump.PushWindowFocusedMessage(m_window, hasFocus);
                else
                    Core.MessagePump.PushWindowUnfocusedMessage(m_window, hasFocus);

                m_window.InvalidateDataCache();
            }

            /// <inheritdoc />
            public bool OnPreDraw()
            {
                return true;
            }
        }
    }
}
