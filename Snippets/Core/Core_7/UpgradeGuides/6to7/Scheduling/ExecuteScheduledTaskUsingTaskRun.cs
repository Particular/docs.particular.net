namespace Core7.UpgradeGuides._6to7.Scheduling
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;

    class ExecuteScheduledTaskUsingTaskRun
    {
        async Task TaskRun(IEndpointInstance endpointInstance, ILog logger)
        {
            #region 6to7ExecuteScheduledTaskUsingTaskRun
            await endpointInstance.ScheduleEvery(
                    timeSpan: TimeSpan.FromMinutes(5),
                    task: pipelineContext =>
                    {
                        Task.Run(() =>
                        {
                            //Do some work here
                            return Task.CompletedTask;
                        })
                        .ContinueWith(task =>
                        {

                            if (task.IsFaulted)
                            {
                                task.Exception.Handle(ex =>
                                {
                                    logger.Error($"Failed to execute MyTask'", ex);
                                    return true;
                                });
                            }
                            else
                            {
                                logger.InfoFormat("MyTask taskcompleted");
                            }
                        }, TaskContinuationOptions.ExecuteSynchronously);

                        return Task.CompletedTask;

                    })
                .ConfigureAwait(false);

            #endregion
        }
    }
}
