using Microsoft.Extensions.Logging;

namespace Maple;
// ShipWithMapleHandler snippet located in solution

class Program
{
    static void Routing(RoutingSettings<LearningTransport> routing)
    {
        #region ShipWithMaple-Routing
        routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
        routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");

        #endregion
    }
}

class ShipOrderWorkflow(ILogger<ShipOrderWorkflow> logger) :
    Saga<ShipOrderWorkflow.ShipOrderData>,
    IAmStartedByMessages<ShipOrder>,
    IHandleMessages<ShipmentAcceptedByMaple>
{

    #region HandleShipOrder
    public async Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("ShipOrderWorkflow for Order [{.OrderId}] - Trying Maple first.", Data.OrderId);

        // Execute order to ship with Maple
        await context.Send(new ShipWithMaple() { OrderId = Data.OrderId });

        // Add timeout to escalate if Maple did not ship in time.
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
    }
    #endregion

    #region ShipWithMaple-ShipmentAccepted
    public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order [{OrderId}] - Successfully shipped with Maple", Data.OrderId);

        Data.ShipmentAcceptedByMaple = true;

        MarkAsComplete();

        return Task.CompletedTask;
    }
    #endregion

    #region ShipWithMaple-Data
    internal class ShipOrderData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool ShipmentAcceptedByMaple { get; set; }
    }
    #endregion

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<ShipOrder>(message => message.OrderId);
    }

    #region ShippingEscalationTimeout
    internal class ShippingEscalation
    {
    }
    #endregion
}