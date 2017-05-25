using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderPlacedHandler :
    IHandleMessages<PlaceOrderResponse>
{
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    #region Reply

    public Task Handle(PlaceOrderResponse placeOrderResponse, IMessageHandlerContext context)
    {
        log.Info($"Received OrderPlaced. OrderId: {placeOrderResponse.OrderId}. Worker: {placeOrderResponse.WorkerName}");

        var confirmOrder = new ConfirmOrder
        {
            OrderId = placeOrderResponse.OrderId
        };

        return context.Reply(confirmOrder);
    }

    #endregion
}