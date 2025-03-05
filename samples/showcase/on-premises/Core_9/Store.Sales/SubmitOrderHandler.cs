using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class SubmitOrderHandler(ILogger<SubmitOrderHandler> logger) :
    IHandleMessages<SubmitOrder>
{
    public Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        logger.LogInformation($"Received an order #{message.OrderNumber} for [{string.Join(", ", message.ProductIds)}] products(s).");

        logger.LogInformation("The credit card values will be encrypted when looking at the messages in the queues");
        logger.LogInformation($"CreditCard Number is {message.CreditCardNumber}");
        logger.LogInformation($"CreditCard Expiration Date is {message.ExpirationDate}");

        // tell the client the order was received
        var orderPlaced = new OrderPlaced
        {
            ClientId = message.ClientId,
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds
        };
        return context.Publish(orderPlaced);
    }
}
