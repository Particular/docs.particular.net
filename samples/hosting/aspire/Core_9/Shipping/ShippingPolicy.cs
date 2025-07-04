using Messages;
using Microsoft.Extensions.Logging;

namespace Shipping;

class ShippingPolicy(ILogger<ShippingPolicy> log) : Saga<ShippingPolicyData>,
    IAmStartedByMessages<OrderBilled>,
    IAmStartedByMessages<OrderPlaced>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<OrderPlaced>(message => message.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId);
    }

    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        log.LogInformation("OrderPlaced message received.");
        Data.IsOrderPlaced = true;

        return ProcessOrder(context);
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        log.LogInformation("OrderBilled message received.");
        Data.IsOrderBilled = true;

        return ProcessOrder(context);
    }

    private async Task ProcessOrder(IMessageHandlerContext context)
    {
        if (Data.IsOrderPlaced && Data.IsOrderBilled)
        {
            await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
            MarkAsComplete();
        }
    }
}
