using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class OrderHandler : IHandleMessages<ClientOrder>
{
    #region Reply

    public async Task Handle(ClientOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine("Handling ClientOrder with ID {0}", message.OrderId);
        await context.Reply(new ClientOrderAccepted { OrderId = message.OrderId });
    }

    #endregion
}