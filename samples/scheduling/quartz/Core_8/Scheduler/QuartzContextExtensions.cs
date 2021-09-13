using NServiceBus;
using Quartz;

#region QuartzContextExtensions

public static class QuartzContextExtensions
{
    public static IEndpointInstance EndpointInstance(this IJobExecutionContext context)
    {
        return (IEndpointInstance) context.Scheduler.Context["EndpointInstance"];
    }

    public static void SetEndpointInstance(this IScheduler scheduler, IEndpointInstance instance)
    {
        scheduler.Context["EndpointInstance"] = instance;
    }
}

#endregion