using Microsoft.Extensions.Logging;

namespace Lesson1.OrderProcessing;

#region ShippingPolicyShipOrder
public class ShipOrder : ICommand
{
    public string OrderId { get; set; }
}
#endregion

public class ShippingPolicyData : ContainSagaData
{
    public string OrderId { get; set; }
    public bool IsOrderPlaced { get; set; }
    public bool IsOrderBilled { get; set; }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("NServiceBus.Sagas", "NSB0006:Message that starts the saga does not have a message mapping", Justification = "<Pending>")]
public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>,
    IAmStartedByMessages<OrderPlaced>,
    IAmStartedByMessages<OrderBilled>
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
        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        Data.IsOrderPlaced = true;
        return ProcessOrder(context);
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        Data.IsOrderBilled = true;
        return ProcessOrder(context);
    }
    #endregion
}

#region EmptyShipOrderHandler
class ShipOrderHandler(ILogger<ShipOrderHandler> logger) : IHandleMessages<ShipOrder>
{

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{OrderId}] - Successfully shipped.", message.OrderId);
        return Task.CompletedTask;
    }
}
#endregion