using System.Data;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"Subscriber has received PlaceOrder command with OrderId {message.OrderId} and UserId {message.UserId}.");
        if(message.UserId != 1)
            throw new DataException($"There is no user with the given UserId:{message.UserId}.");
        return Task.CompletedTask;
    }
}