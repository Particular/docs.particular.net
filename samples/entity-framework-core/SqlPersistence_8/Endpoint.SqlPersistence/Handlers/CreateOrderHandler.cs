using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CreateOrderHandler(ReceiverDataContext dataContext, ILogger<CreateOrderHandler> logger) :
    IHandleMessages<OrderSubmitted>
{
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

        logger.LogInformation($"Order {message.OrderId} worth {message.Value} created.");

        return Task.CompletedTask;
    }
}