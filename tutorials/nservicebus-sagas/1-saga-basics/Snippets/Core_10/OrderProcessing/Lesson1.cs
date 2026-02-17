using Microsoft.Extensions.Logging;

namespace OrderProcessing;

#region ShippingPolicyShipOrder
public class ShipOrder : ICommand
{
    public string? OrderId { get; set; }
}
#endregion

public class ShippingPolicyData : ContainSagaData
{
    public string? OrderId { get; set; }

    public bool IsOrderPlaced { get; set; }

    public bool IsOrderBilled { get; set; }
}

#pragma warning disable NSB0006 // Message that starts the saga does not have a message mapping
public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>, IAmStartedByMessages<OrderPlaced>, IAmStartedByMessages<OrderBilled>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {

    }

    #region ShippingPolicyProcessOrder
    private async Task ProcessOrder(IMessageHandlerContext context)
    {
        if (Data.IsOrderPlaced && Data.IsOrderBilled)
        {
            await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
            MarkAsComplete();
        }
    }
    #endregion

    #region ShippingPolicyFinalHandleWithProcessOrder
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {orderId}.", message.OrderId);
        Data.IsOrderPlaced = true;
        return ProcessOrder(context);
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {orderId}.", message.OrderId);
        Data.IsOrderBilled = true;
        return ProcessOrder(context);
    }
    #endregion
}

#pragma warning restore NSB0006 // Message that starts the saga does not have a message mapping

#region EmptyShipOrderHandler
class ShipOrderHandler(ILogger<ShipOrderHandler> logger) : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{orderId}] - Successfully shipped.", message.OrderId);
        return Task.CompletedTask;
    }
}
#endregion