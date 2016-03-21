using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
{
    static ILog log = LogManager.GetLogger<SubmitOrderHandler>();

    public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
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

        //tell the client that we received the order
        await context.Publish<OrderPlaced>(o =>
            {
                o.ClientId = message.ClientId;
                o.OrderNumber = message.OrderNumber;
                o.ProductIds = message.ProductIds;
            });
    }
    
}