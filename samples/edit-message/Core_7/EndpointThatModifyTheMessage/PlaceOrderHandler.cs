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
        log.Info($"EndpointThatModifyTheMessage has received PlaceOrder command with OrderId {message.OrderId} and UserId {message.UserId}.");
        var properUserId = 1;
        log.Info($"The message will be modified. Old value UserId:{message.UserId} => UserId:{properUserId} and send to it's originating endpoint");
        message.UserId = properUserId;

        var options = new SendOptions();
        options.SetDestination("Samples.EditMessage.Subscriber");
        return context.Send(message, options);
    }
}