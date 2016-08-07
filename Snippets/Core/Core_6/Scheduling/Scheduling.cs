namespace Core6.Scheduling
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class Scheduling
    {
        Scheduling(IEndpointInstance endpointInstance)
        {
            #region ScheduleTask
            // To send a message every 5 minutes
            endpointInstance.ScheduleEvery(
                timeSpan: TimeSpan.FromMinutes(5),
                task: context =>
                {
                    var message = new CallLegacySystem();
                    return context.Send(message);
                });

            // Name a schedule task and invoke it every 5 minutes
            endpointInstance.ScheduleEvery(
                timeSpan: TimeSpan.FromMinutes(5),
                name: "MyCustomTask",
                task: SomeCustomMethod);

            #endregion
        }

        Task SomeCustomMethod(IPipelineContext context)
        {
            return Task.FromResult(0);
        }

    }
    class CallLegacySystem
    {
    }
}