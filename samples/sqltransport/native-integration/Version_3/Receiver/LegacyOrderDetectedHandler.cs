using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class LegacyOrderDetectedHandler : IHandleMessages<LegacyOrderDetected>
{
    static ILog log = LogManager.GetLogger<LegacyOrderDetectedHandler>();

    public Task Handle(LegacyOrderDetected message, IMessageHandlerContext context)
    {
        log.InfoFormat("Legacy order with id {0} detected", message.OrderId);

        //Get the order details from the database and publish an event

        return Task.FromResult(0);
    }

}