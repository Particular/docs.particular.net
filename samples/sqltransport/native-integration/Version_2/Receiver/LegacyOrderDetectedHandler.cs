using NServiceBus;
using NServiceBus.Logging;

public class LegacyOrderDetectedHandler : IHandleMessages<LegacyOrderDetected>
{
    static ILog log = LogManager.GetLogger<LegacyOrderDetectedHandler>();
    public void Handle(LegacyOrderDetected message)
    {
        log.InfoFormat("Legacy order with id {0} detected", message.OrderId);

        //Get the order details from the database and publish an event
    }
}