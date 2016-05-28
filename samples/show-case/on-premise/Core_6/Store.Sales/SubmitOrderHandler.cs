using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
{
    static ILog log = LogManager.GetLogger<SubmitOrderHandler>();

    public Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.InfoFormat($"We have received an order #{message.OrderNumber} for [{string.Join(", ", message.ProductIds)}] products(s).");

        log.InfoFormat("The credit card values will be encrypted when looking at the messages in the queues");
        log.InfoFormat($"CreditCard Number is {message.EncryptedCreditCardNumber}");
        log.InfoFormat($"CreditCard Expiration Date is {message.EncryptedExpirationDate}");

        // tell the client the order was received
        return context.Publish<OrderPlaced>(o =>
            {
                o.ClientId = message.ClientId;
                o.OrderNumber = message.OrderNumber;
                o.ProductIds = message.ProductIds;
            });
    }

}