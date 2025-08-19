using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using NServiceBus;

class OrderReceivedHandler : IHandleMessages<OrderReceived>
{
    readonly ILogger<OrderReceivedHandler> logger;

    public OrderReceivedHandler(ILogger<OrderReceivedHandler> logger)
    {
        this.logger = logger;
    }

    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received {Event} event for order #{OrderId}", nameof(OrderReceived), message.OrderId);

        var session = context.SynchronizedStorageSession.CosmosPersistenceSession();

        var order = await session.Container.ReadItemAsync<OrderDocument>(message.OrderId, session.PartitionKey);
        order.Resource.Status = "Accepted";

        // update the document atomically with consuming the message
        session.Batch.ReplaceItem(message.OrderId, order.Resource);
    }
}