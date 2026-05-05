using System.Threading.Tasks;
using NServiceBus;

#region SendMessageJob
public static class SendMessageJob
{
    public static Task Run()
    {
        var messageSession = EndpointHelper.MessageSession;
        return messageSession.Send("Samples.HangfireScheduler.Receiver", new MyMessage());
    }
}
#endregion