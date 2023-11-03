using Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Marketing;

public class OrderPlacedEventHandler : IHandleMessages<IOrderPlaced>
{
    private static readonly ILog log = LogManager.GetLogger<OrderPlacedEventHandler>();

    public Task Handle(IOrderPlaced message, IMessageHandlerContext context)
    {
        log.Info($"Verifying whether customer is eligible for a discount code...");

        log.Info($"Customer was not yet eligible for a discount code...");
        return Task.CompletedTask;
    }
}