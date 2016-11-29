using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderPlacedHandler :
    IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    #region Reply

    public Task Handle(OrderPlaced orderPlaced, IMessageHandlerContext context)
    {
        log.Info($"Received OrderPlaced. OrderId: {orderPlaced.OrderId}. Worker: {orderPlaced.WorkerName}");

        var confirmOrder = new ConfirmOrder
        {
            OrderId = orderPlaced.OrderId
        };

        return context.Reply(confirmOrder);
    }

    #endregion
}