namespace Core8.Scheduling
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Scheduling
    {
        static ILog log = LogManager.GetLogger<Scheduling>();

        async Task Simple(IEndpointInstance endpointInstance)
        {
            #region ScheduleTask

            // To send a message every 5 minutes
            await endpointInstance.ScheduleEvery(
                    timeSpan: TimeSpan.FromMinutes(5),
                    task: pipelineContext =>
                    {
                        // use the pipelineContext parameter to send messages
                        var message = new CallLegacySystem();
                        return pipelineContext.Send(message);
                    })
                .ConfigureAwait(false);

            // Name a schedule task and invoke it every 5 minutes
            await endpointInstance.ScheduleEvery(
                    timeSpan: TimeSpan.FromMinutes(5),
                    name: "MyCustomTask",
                    task: pipelineContext =>
                    {
                        log.Info("Custom Task executed");
                        return Task.CompletedTask;
                    })
                .ConfigureAwait(false);

            #endregion
        }

        Task SomeCustomMethod(IPipelineContext context)
        {
            return Task.CompletedTask;
        }

    }
    class CallLegacySystem
    {
    }
}