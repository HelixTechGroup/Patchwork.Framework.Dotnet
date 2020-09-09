using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Patchwork.Framework.Messaging;
using Shin.Framework.Extensions;

namespace Patchwork.Framework
{
    [Preserve(AllMembers = true)]
    internal class ActivityStatusTracker : Java.Lang.Object, Application.IActivityLifecycleCallbacks
    {
        public event EventHandler<ActivityEventArgs> ActivityStateChanged;

        public Activity Current { get; private set; }

        public Activity Previous { get; private set; }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            Previous = Current;
            Current = activity;
            OnStateChanged(activity, ActivityEvent.Created);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            Previous = Current;
            if (Current == activity)
                Current = null;
            OnStateChanged(activity, ActivityEvent.Destroyed);
        }

        public void OnActivityPaused(Activity activity)
        {
            Previous = Current;
            if (Current == activity)
                Current = null;
            OnStateChanged(activity, ActivityEvent.Paused);
        }

        public void OnActivityResumed(Activity activity)
        {
            Previous = Current;
            Current = activity;
            OnStateChanged(activity, ActivityEvent.Resumed);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            Previous = Current;
            Current = activity;
            OnStateChanged(activity, ActivityEvent.SaveInstanceState);
        }

        public void OnActivityStarted(Activity activity)
        {
            Previous = Current;
            Current = activity;
            OnStateChanged(activity, ActivityEvent.Started);
        }

        public void OnActivityStopped(Activity activity)
        {
            Previous = Current;
            if (Current == activity)
                Current = null;
            OnStateChanged(activity, ActivityEvent.Stopped);
        }

        public async Task<Activity> WaitForActivityAsync(CancellationToken cancelToken = default)
        {
            if (Current != null)
                return Current;

            var tcs = new TaskCompletionSource<Activity>(cancelToken);
            var handler = new EventHandler<ActivityEventArgs>((sender, args) =>
            {
                if (args.Event == ActivityEvent.Created || args.Event == ActivityEvent.Resumed)
                    tcs.TrySetResult(args.Activity);
            });

            try
            {
                await using (cancelToken.Register(() => tcs.TrySetCanceled()))
                {
                    ActivityStateChanged += handler;
                    return await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                ActivityStateChanged -= handler;
            }
        }

        protected void OnStateChanged(Activity activity, ActivityEvent ev)
        {
            ActivityStateChanged.Raise(this, new ActivityEventArgs(activity, ev));
        }
    }
}
