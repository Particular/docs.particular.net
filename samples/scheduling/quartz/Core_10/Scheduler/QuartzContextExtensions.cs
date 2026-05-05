using NServiceBus;
using Quartz;

#region QuartzContextExtensions

public static class QuartzContextExtensions
{
    public static IMessageSession MessageSession(this IJobExecutionContext context)
    {
        return (IMessageSession) context.Scheduler.Context["MessageSession"];
    }

    public static void SetMessageSession(this IScheduler scheduler, IMessageSession messageSession)
    {
        scheduler.Context["MessageSession"] = messageSession;
    }
}

#endregion