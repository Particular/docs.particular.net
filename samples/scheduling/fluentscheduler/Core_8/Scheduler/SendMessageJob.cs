using FluentScheduler;
using NServiceBus;

#region SendMessageJob
public class SendMessageJob : IJob
{
    IEndpointInstance endpoint;

    public SendMessageJob(IEndpointInstance endpoint)
    {
        this.endpoint = endpoint;
    }

    public void Execute()
    {
        endpoint.Send("Samples.FluentScheduler.Receiver", new MyMessage())
            .GetAwaiter().GetResult();
    }
}
#endregion