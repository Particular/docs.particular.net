using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class OrderHandler (ILogger<OrderHandler> logger):
    IHandleMessages<ClientOrder>
{
    #region Reply

    public Task Handle(ClientOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling ClientOrder with ID {OrderId}", message.OrderId);
        var clientOrderAccepted = new ClientOrderResponse
        {
            OrderId = message.OrderId
        };
        return context.Reply(clientOrderAccepted);
    }

    #endregion
}