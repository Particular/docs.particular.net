using System;
using Messages;
using NServiceBus;

public class OrderHandler :
    IHandleMessages<ClientOrder>
{
    IBus bus;

    public OrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    #region Reply

    public void Handle(ClientOrder message)
    {
        Console.WriteLine($"Handling ClientOrder with ID {message.OrderId}");
        var clientOrderAccepted = new ClientOrderAccepted
        {
            OrderId = message.OrderId
        };
        bus.Reply(clientOrderAccepted);
    }

    #endregion
}