using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class CreateOrderHandler(ReceiverDataContext dataContext) :
    IHandleMessages<OrderSubmitted>
{
    static readonly ILog log = LogManager.GetLogger<CreateOrderHandler>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        #region StoreOrder

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        dataContext.Orders.Add(order);
        await dataContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

        #endregion

        log.Info($"Order {message.OrderId} worth {message.Value} created.");
    }
}