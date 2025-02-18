using System.Threading.Tasks;
using Messages;
using NServiceBus;
using System;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        // This is normally where some business logic would occur

        // Uncomment to test throwing a systemic exception
        throw new Exception("BOOM");

        //Uncomment to test throwing a transient exception
        //if (Random.Shared.Next(0, 5) == 0)
        //{
        //    throw new Exception("Oops");
        //}

        //var orderPlaced = new OrderPlaced
        //{
        //    OrderId = message.OrderId
        //};
        //return context.Publish(orderPlaced);
    }
}