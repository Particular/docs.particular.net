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
        log.Info($"CreditCard Number is {message.EncryptedCreditCardNumber}");
        log.Info($"CreditCard Expiration Date is {message.EncryptedExpirationDate}");

        // tell the client the order was received 
        bus.Publish<OrderPlaced>(o =>
            {
                o.ClientId = message.ClientId;
                o.OrderNumber = message.OrderNumber;
                o.ProductIds = message.ProductIds;
            });
    }
}
