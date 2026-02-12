using Microsoft.Extensions.Logging;

namespace SagaMappings;

#region ExtendedShippingPolicyData
public class ShippingPolicyData : ContainSagaData
{
    public string? OrderId { get; set; }
    public bool IsOrderPlaced { get; set; }
    public bool IsOrderBilled { get; set; }
}
#endregion

public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>,
    IAmStartedByMessages<OrderPlaced>, // This can start the saga
    IHandleMessages<OrderBilled>       // But surely, not this one!?
{

    #region ShippingPolicyFinalMappings
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<OrderPlaced>(message => message.OrderId)
            .ToMessage<OrderBilled>(message => message.OrderId);
    }
    #endregion

    #region ShippingPolicyCorrelationAutoPopulation
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        // DON'T NEED THIS! NServiceBus does this for us.
        Data.OrderId = message.OrderId;

        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        Data.IsOrderPlaced = true;
        return Task.CompletedTask;
    }
    #endregion

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
        return Task.CompletedTask;
    }
}