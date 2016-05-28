using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class OrderHandler : IHandleMessages<ClientOrder>
{
    #region Reply

    public Task Handle(ClientOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Handling ClientOrder with ID {message.OrderId}");
        var clientOrderAccepted = new ClientOrderAccepted
        {
            OrderId = message.OrderId
        };
        return context.Reply(clientOrderAccepted);
    }

    #endregion
}