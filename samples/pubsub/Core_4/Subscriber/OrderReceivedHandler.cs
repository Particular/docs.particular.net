using NServiceBus;
using NServiceBus.Logging;

public class OrderReceivedHandler :
    IHandleMessages<OrderReceived>
{
    static ILog log = LogManager.GetLogger(typeof(OrderReceivedHandler));

    public void Handle(OrderReceived message)
    {
        log.Info($"Subscriber has received the OrderReceived event with OrderId: {message.OrderId}.");
    }
}