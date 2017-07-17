using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class CreateOrderHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<CreateOrderHandler>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreOrder

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        context.DataContext().Orders.Add(order);

        #endregion

        return Task.CompletedTask;
    }
}