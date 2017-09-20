using System.Threading.Tasks;
using NServiceBus;
using Quartz;

#region SendMessageJob
public class SendMessageJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var endpointInstance = context.EndpointInstance();
        return endpointInstance.Send("Samples.QuartzScheduler.Receiver", new MyMessage());
    }
}
#endregion