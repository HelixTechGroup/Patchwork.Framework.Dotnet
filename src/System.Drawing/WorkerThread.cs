using System.Threading;

namespace System.Drawing
{
    class WorkerThread {

        private EventHandler frameChangeHandler;
        private AnimateEventArgs animateEventArgs;
        private int[] delay;

        public WorkerThread (EventHandler frmChgHandler, AnimateEventArgs aniEvtArgs, int[] delay)
        {
            frameChangeHandler = frmChgHandler;
            animateEventArgs = aniEvtArgs;
            this.delay = delay;
        }

        public void LoopHandler ()
        {
            try {
                int n = 0;
                while (true) {
                    Thread.Sleep (delay [n++]);
                    frameChangeHandler (null, animateEventArgs);
                    if (n == delay.Length)
                        n = 0;
                }
            }
            catch (ThreadAbortException) {
                Thread.ResetAbort (); // we're going to finish anyway
            }
        }
    }
}