using NServiceBus;
using NServiceBus.Logging;

public class LegacyOrderDetectedHandler :
    IHandleMessages<LegacyOrderDetected>
{
    static ILog log = LogManager.GetLogger<LegacyOrderDetectedHandler>();
    public void Handle(LegacyOrderDetected message)
    {
        log.Info($"Legacy order with id {message.OrderId} detected");
        // Get the order details from the database and publish an event
    }
}