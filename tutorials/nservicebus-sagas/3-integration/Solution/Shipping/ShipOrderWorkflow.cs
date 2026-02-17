using Messages;
using Microsoft.Extensions.Logging;

namespace Shipping;

class ShipOrderWorkflow(ILogger<ShipOrderWorkflow> logger) : Saga<ShipOrderWorkflow.ShipOrderData>, IAmStartedByMessages<ShipOrder>, IHandleMessages<ShipmentAcceptedByMaple>, IHandleMessages<ShipmentAcceptedByAlpine>, IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<ShipOrder>(message => message.OrderId);
    }

    public async Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("ShipOrderWorkflow for Order [{orderId}] - Trying Maple first.", Data.OrderId);

        // Execute order to ship with Maple
        await context.Send(new ShipWithMaple() { OrderId = Data.OrderId });

        // Add timeout to escalate if Maple did not ship in time.
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
    }

    public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
    {
        if (!Data.ShipmentOrderSentToAlpine)
        {
            logger.LogInformation("Order [{orderId}] - Successfully shipped with Maple", Data.OrderId);

            Data.ShipmentAcceptedByMaple = true;

            MarkAsComplete();
        }

        return Task.CompletedTask;
    }

    public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{orderId}] - Successfully shipped with Alpine", Data.OrderId);

        Data.ShipmentAcceptedByAlpine = true;

        MarkAsComplete();

        return Task.CompletedTask;
    }

    public async Task Timeout(ShippingEscalation timeout, IMessageHandlerContext context)
    {
        if (!Data.ShipmentAcceptedByMaple)
        {
            if (!Data.ShipmentOrderSentToAlpine)
            {
                logger.LogInformation("Order [{orderId}] - No answer from Maple, let's try Alpine.", Data.OrderId);
                Data.ShipmentOrderSentToAlpine = true;
                await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId });
                await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
            }
            else if (!Data.ShipmentAcceptedByAlpine) // No response from Maple nor Alpine
            {
                logger.LogWarning("Order [{orderId}] - No answer from Maple/Alpine. We need to escalate!", Data.OrderId);

                // escalate to Warehouse Manager!
                await context.Publish<ShipmentFailed>();

                MarkAsComplete();
            }
        }
    }

    internal class ShipOrderData : ContainSagaData
    {
        public string? OrderId { get; set; }

        public bool ShipmentAcceptedByMaple { get; set; }

        public bool ShipmentOrderSentToAlpine { get; set; }

        public bool ShipmentAcceptedByAlpine { get; set; }
    }

    internal class ShippingEscalation
    {
    }
}