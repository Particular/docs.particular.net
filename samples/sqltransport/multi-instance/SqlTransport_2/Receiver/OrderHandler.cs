using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class OrderHandler :
    IHandleMessages<ClientOrder>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<OrderHandler>();

    public OrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    #region Reply

    public void Handle(ClientOrder message)
    {
        log.Info($"Handling ClientOrder with ID {message.OrderId}");
        var clientOrderAccepted = new ClientOrderAccepted
        {
            OrderId = message.OrderId
        };
        bus.Reply(clientOrderAccepted);
    }

    #endregion
}