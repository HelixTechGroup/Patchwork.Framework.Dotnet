using System;
using System.Collections.Generic;
using System.Text;
using Android.App;

namespace Patchwork.Framework
{
    public class ActivityEventArgs : EventArgs
    {
        internal ActivityEventArgs(Activity activity, ActivityEvent ev)
        {
            Event = ev;
            Activity = activity;
        }

        public ActivityEvent Event { get; }
        public Activity Activity { get; }
    }
}
