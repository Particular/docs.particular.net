using System.Threading.Tasks;
using NServiceBus;

#region SendMessageJob
public static class SendMessageJob
{
    public static Task Run()
    {
        var endpoint = EndpointHelper.Instance;
        return endpoint.Send("Samples.HangfireScheduler.Receiver", new MyMessage());
    }
}
#endregion