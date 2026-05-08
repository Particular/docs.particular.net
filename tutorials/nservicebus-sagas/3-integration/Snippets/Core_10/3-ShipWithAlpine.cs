using Microsoft.Extensions.Logging;

namespace Alpine;
// ShipWithAlpineHandler snippet located in solution

class Program
{
    public static void Routing(RoutingSettings<LearningTransport> routing)
    {
        #region ShipWithAlpine-Routing
        routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
        routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");
        routing.RouteToEndpoint(typeof(ShipWithAlpine), "Shipping");
        #endregion
    }
}

class ShipOrderWorkflow(ILogger<ShipOrderWorkflow> logger) : Saga<ShipOrderWorkflow.ShipOrderData>, IAmStartedByMessages<ShipOrder>, IHandleMessages<ShipmentAcceptedByMaple>, IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        // Stub only
        return Task.CompletedTask;
    }

    #region ShipWithMaple-ShipmentAcceptedRevision
    public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
    {
        if (!Data.ShipmentOrderSentToAlpine)
        {
            logger.LogInformation("Order [{OrderId}] - Successfully shipped with Maple", Data.OrderId);

            Data.ShipmentAcceptedByMaple = true;

            MarkAsComplete();
        }

        return Task.CompletedTask;
    }
    #endregion

    #region ShippingEscalation
    public async Task Timeout(ShippingEscalation timeout, IMessageHandlerContext context)
    {
        if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
        {
            logger.LogInformation("Order [{OrderId}] - We didn't receive answer from Maple, let's try Alpine.", Data.OrderId);
            Data.ShipmentOrderSentToAlpine = true;
            await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId });
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
        }
    }
    #endregion

    #region ShipWithAlpine-Data
    internal class ShipOrderData : ContainSagaData
    {
        public string? OrderId { get; set; }

        public bool ShipmentAcceptedByMaple { get; set; }

        public bool ShipmentOrderSentToAlpine { get; set; }
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