using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class OrderHandler :
    IHandleMessages<ClientOrder>
{
    static ILog log = LogManager.GetLogger<OrderHandler>();

    #region Reply

    public Task Handle(ClientOrder message, IMessageHandlerContext context)
    {
        log.Info($"Handling ClientOrder with ID {message.OrderId}");
        var clientOrderAccepted = new ClientOrderAccepted
        {
            OrderId = message.OrderId
        };
        return context.Reply(clientOrderAccepted);
    }

    #endregion
}