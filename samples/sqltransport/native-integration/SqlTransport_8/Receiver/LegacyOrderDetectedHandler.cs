using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class LegacyOrderDetectedHandler(ILogger<LegacyOrderDetectedHandler> logger) :
    IHandleMessages<LegacyOrderDetected>
{
      public Task Handle(LegacyOrderDetected message, IMessageHandlerContext context)
    {
        logger.LogInformation("Legacy order with id {OrderId} detected", message.OrderId);
        // Get the order details from the database and publish an event
        return Task.CompletedTask;
    }
}