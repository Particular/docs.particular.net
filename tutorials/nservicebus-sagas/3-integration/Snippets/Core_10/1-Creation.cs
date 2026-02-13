using Microsoft.Extensions.Logging;

#region Creation-SagaStart
class ShipOrderWorkflow(ILogger<ShipOrderWorkflow> logger) :
    Saga<ShipOrderWorkflow.ShipOrderData>,
    IAmStartedByMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling ShipOrder");
        return Task.CompletedTask;
    }

    internal class ShipOrderData : ContainSagaData
    {
        public string? OrderId { get; set; }
    }
    // ...
    #endregion

    #region Creation-ConfigureHowToFindSaga
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<ShipOrder>(message => message.OrderId);
    }
    #endregion
}
