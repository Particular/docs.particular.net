using Messages;
using Microsoft.Extensions.Logging;

namespace Shipping;

class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>, IAmStartedByMessages<OrderBilled>, IAmStartedByMessages<OrderPlaced>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<OrderPlaced>(message => message.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId);
    }

    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        Data.IsOrderPlaced = true;
        return ProcessOrder(context);
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderBilled message received for {OrderId}.", message.OrderId);
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

class ShippingPolicyData : ContainSagaData
{
    public string? OrderId { get; set; }

    public bool IsOrderPlaced { get; set; }

    public bool IsOrderBilled { get; set; }
}
