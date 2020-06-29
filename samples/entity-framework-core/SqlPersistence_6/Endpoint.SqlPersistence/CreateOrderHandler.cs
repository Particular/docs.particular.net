using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class CreateOrderHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<CreateOrderHandler>();
    ReceiverDataContext dataContext;

    public CreateOrderHandler(ReceiverDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

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