using NServiceBus;
using NServiceBus.Logging;

#region CustomSendMessageAfterEndpointStart
public class SendMessageAfterEndpointStart : IRunAfterEndpointStart
{
    static ILog log = LogManager.GetLogger<SendMessageAfterEndpointStart>();
    public void Run(IBus bus)
    {
        log.Info("Sending Message.");
        var myMessage = new MyMessage();
        bus.SendLocal(myMessage);
    }
}
#endregion