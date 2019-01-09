using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class SimpleMessageHandler :
    IHandleMessages<SimpleMessage>
{
    static ILog log = LogManager.GetLogger<SimpleMessageHandler>();

    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region RepairAndForward

        log.Info($"Repairing message with Id = {message.Id}.");

        message.Id = message.Id.ToUpperInvariant();

        log.Info($"Forwarding repaired message with Id = {message.Id} to the Receiver.");

        return context.Send("FixMalformedMessages.Receiver", message);
        
        #endregion
    }
}