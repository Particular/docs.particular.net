using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
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

        log.InfoFormat("We have received an order #{0} for [{1}] products(s).", message.OrderNumber,
                            string.Join(", ", message.ProductIds));

        log.InfoFormat("The credit card values will be encrypted when looking at the messages in the queues");
        log.InfoFormat("CreditCard Number is {0}", message.EncryptedCreditCardNumber);
        log.InfoFormat("CreditCard Expiration Date is {0}", message.EncryptedExpirationDate);

        // tell the client the order was received 
        bus.Publish<OrderPlaced>(o =>
            {
                o.ClientId = message.ClientId;
                o.OrderNumber = message.OrderNumber;
                o.ProductIds = message.ProductIds;
            });
    }
}