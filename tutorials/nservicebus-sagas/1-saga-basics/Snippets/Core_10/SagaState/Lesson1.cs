using Microsoft.Extensions.Logging;

namespace SagaState;

#region ShippingPolicyAugmentedWithData
public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>,
    IHandleMessages<OrderPlaced>,
    IHandleMessages<OrderBilled>
#endregion
{

    #region EmptyConfigureHowToFindSaga
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        // TODO
    }
    #endregion

    #region HandleBasicImplementation
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        Data.IsOrderPlaced = true;
        return Task.CompletedTask;
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        Data.IsOrderBilled = true;
        return Task.CompletedTask;
    }
    #endregion
}