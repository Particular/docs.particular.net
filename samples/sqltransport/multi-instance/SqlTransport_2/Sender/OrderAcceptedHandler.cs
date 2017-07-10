using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<ClientOrderAccepted>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public OrderAcceptedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(ClientOrderAccepted message)
    {
        log.Info($"Received ClientOrderAccepted for ID {message.OrderId}");
    }
}