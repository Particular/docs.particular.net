using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

namespace AlpineAccepted;

class ShipOrderWorkflow(ILogger<ShipOrderWorkflow> logger) :
    Saga<ShipOrderWorkflow.ShipOrderData>,
    IAmStartedByMessages<ShipOrder>,
    IHandleMessages<ShipmentAcceptedByMaple>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        // Stub
        return Task.CompletedTask;
    }

    public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
    {
        // Stub
        return Task.CompletedTask;
    }

    #region ShipmentAcceptedByAlpine
    public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{OrderId}] - Successfully shipped with Alpine", Data.OrderId);

        Data.ShipmentAcceptedByAlpine = true;

        MarkAsComplete();

        return Task.CompletedTask;
    }
    #endregion

    #region AcceptedByAlpine-Data
    internal class ShipOrderData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool ShipmentAcceptedByMaple { get; set; }
        public bool ShipmentOrderSentToAlpine { get; set; }
        public bool ShipmentAcceptedByAlpine { get; set; }
    }
    #endregion

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<ShipOrder>(message => message.OrderId);
    }

    internal class ShippingEscalation
    {
    }
}