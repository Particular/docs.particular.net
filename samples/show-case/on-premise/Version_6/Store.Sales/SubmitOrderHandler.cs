using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Commands;
using Store.Messages.Events;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
{

    public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Console.WriteLine("We have received an order #{0} for [{1}] products(s).", message.OrderNumber,
                            string.Join(", ", message.ProductIds));

        Console.WriteLine("The credit card values will be encrypted when looking at the messages in the queues");
        Console.WriteLine("CreditCard Number is {0}", message.EncryptedCreditCardNumber);
        Console.WriteLine("CreditCard Expiration Date is {0}", message.EncryptedExpirationDate);

        //tell the client that we received the order
        await context.Publish<OrderPlaced>(o =>
            {
                o.ClientId = message.ClientId;
                o.OrderNumber = message.OrderNumber;
                o.ProductIds = message.ProductIds;
            });
    }
    
}