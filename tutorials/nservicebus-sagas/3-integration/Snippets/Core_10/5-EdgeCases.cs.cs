using Microsoft.Extensions.Logging;

namespace EdgeCases;

class ShipOrderWorkflow(ILogger<ShipOrderWorkflow> logger) : Saga<ShipOrderWorkflow.ShipOrderData>, IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>
{
    #region EdgeCases-ShipmentFailed
    public async Task Timeout(ShippingEscalation timeout, IMessageHandlerContext context)
    {
        if (!Data.ShipmentAcceptedByMaple)
        {
            if (!Data.ShipmentOrderSentToAlpine)
            {
                logger.LogInformation("Order [{OrderId}] - No answer from Maple, let's try Alpine.", Data.OrderId);
                Data.ShipmentOrderSentToAlpine = true;
                await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId });
                await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation());
            }
            else if (!Data.ShipmentAcceptedByAlpine) // No response from Maple nor Alpine
            {
                logger.LogWarning("Order [{OrderId}] - No answer from Maple/Alpine. We need to escalate!", Data.OrderId);

                // escalate to Warehouse Manager!
                await context.Publish<ShipmentFailed>();

                MarkAsComplete();
            }
        }
    }
    #endregion

    public void FakeMethod()
    {
        #region EdgeCases-IfShipmentAccepted
        if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
        #endregion
        {
        }
    }

    internal class ShipOrderData : ContainSagaData
    {
        public string? OrderId { get; set; }

        public bool ShipmentAcceptedByMaple { get; set; }

        public bool ShipmentOrderSentToAlpine { get; set; }

        public bool ShipmentAcceptedByAlpine { get; set; }
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<ShipOrder>(message => message.OrderId);
    }

    internal class ShippingEscalation
    {
    }
}