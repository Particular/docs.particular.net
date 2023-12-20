using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class CreateOrderHandler(ReceiverDataContext dataContext) :
    IHandleMessages<OrderSubmitted>
{
    static readonly ILog log = LogManager.GetLogger<CreateOrderHandler>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        #region StoreOrder

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        dataContext.Orders.Add(order);

        #endregion

        log.Info($"Order {message.OrderId} worth {message.Value} created.");

        return Task.CompletedTask;
    }
}