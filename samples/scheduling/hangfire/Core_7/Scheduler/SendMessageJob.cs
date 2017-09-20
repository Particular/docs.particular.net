using System.Threading.Tasks;
using NServiceBus;

#region SendMessageJob
public static class SendMessageJob
{
    public static Task Run()
    {
        var endpointInstance = EndpointHelper.Instance;
        return endpointInstance.Send("Samples.HangfireScheduler.Receiver", new MyMessage());
    }
}
#endregion