#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using Android.Runtime;
using Android.Views;
using Patchwork.Framework.Extensions;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform.Window;
using Shin.Framework.ComponentModel;
using Shin.Framework.Extensions;
using Object = Java.Lang.Object;
#endregion

namespace Patchwork.Framework.Platform
{
    public sealed partial class AndroidWindow
    {
        #region Nested Types
        [Preserve(AllMembers = true)]
        private class PatchworkViewListener : Object,
                                                View.IOnLayoutChangeListener,
                                                View.IOnFocusChangeListener
        {
            #region Members
            private readonly View m_view;
            private readonly INWindow m_window;
            #endregion

            public PatchworkViewListener(INWindow window, View view)
            {
                m_view = view;
                m_window = window;
                m_view.AddOnLayoutChangeListener(this);
            }

            #region Methods
            /// <inheritdoc />
            public void OnLayoutChange(View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                if (v != m_view)
                    return;

                var oldWidth = oldRight - oldLeft;
                var oldHeight = oldBottom - oldTop;
                var changed = false;
                if (v.Width != oldWidth || v.Height != oldHeight)
                {
                    changed = true;
                    var newSize = new Size(v.Width, v.Height);
                    var oldSize = new Size(oldWidth, oldHeight);
                    Core.MessagePump.PushWindowResizingMessage(m_window, oldSize, newSize);
                    Core.MessagePump.PushWindowResizedMessage(m_window, newSize, newSize, oldSize);
                }

                if (left != oldLeft || top != oldTop)
                {
                    changed = true;
                    var newPos = new Point(left, top);
                    var oldPos = new Point(oldLeft, oldTop);
                    Core.MessagePump.PushWindowMovingMessage(m_window, oldPos, newPos);
                    Core.MessagePump.PushWindowMovedMessage(m_window, newPos, newPos, oldPos);
                }

                if (changed)
                    m_window.InvalidateDataCache();
            }

            /// <inheritdoc />
            public void OnFocusChange(View v, bool hasFocus)
            {
                if (v != m_view)
                    return;
            }
            #endregion
        }
        #endregion
    }
}