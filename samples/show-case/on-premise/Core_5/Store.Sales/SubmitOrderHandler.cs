using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class SubmitOrderHandler :
    IHandleMessages<SubmitOrder>
{
    static ILog log = LogManager.GetLogger<SubmitOrderHandler>();
    IBus bus;

    public SubmitOrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(SubmitOrder message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.Info($"Received an order #{message.OrderNumber} for [{string.Join(", ", message.ProductIds)}] products(s).");

        log.Info("The credit card values will be encrypted when looking at the messages in the queues");
        log.Info($"CreditCard Number is {message.CreditCardNumber}");
        log.Info($"CreditCard Expiration Date is {message.ExpirationDate}");

        // tell the client the order was received
        var orderPlaced = new OrderPlaced
        {
            ClientId = message.ClientId,
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds
        };
        bus.Publish(orderPlaced);
    }
}
