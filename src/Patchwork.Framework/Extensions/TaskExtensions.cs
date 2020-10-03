namespace Patchwork.Framework.Extensions
{
    public static class TaskExtensions
    {
        //public static async Task WhenAllEx(this Task task, ICollection<Task> tasks, Action<ICollection<Task>> reportProgressAction)
        //{
        //    // get Task which completes when all 'tasks' have completed
        //    var whenAllTask = Task.WhenAll(tasks);
        //    for (;;)
        //    {
        //        // get Task which completes after 250ms
        //        var timer = Task.Delay(250); // you might want to make this configurable
        //        // Wait until either all tasks have completed OR 250ms passed
        //        await Task.WhenAny(whenAllTask, timer);
        //        // if all tasks have completed, complete the returned task
        //        if (whenAllTask.IsCompleted)
        //        {
        //            return;
        //        }

        //        // Otherwise call progress report lambda and do another round
        //        reportProgressAction(tasks);
        //    }
        //}
    }
}