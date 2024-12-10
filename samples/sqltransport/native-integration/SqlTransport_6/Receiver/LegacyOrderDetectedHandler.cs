using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class LegacyOrderDetectedHandler :
    IHandleMessages<LegacyOrderDetected>
{
    static ILog log = LogManager.GetLogger<LegacyOrderDetectedHandler>();

    public Task Handle(LegacyOrderDetected message, IMessageHandlerContext context)
    {
        log.Info($"Legacy order with id {message.OrderId} detected");
        // Get the order details from the database and publish an event
        return Task.CompletedTask;
    }
}