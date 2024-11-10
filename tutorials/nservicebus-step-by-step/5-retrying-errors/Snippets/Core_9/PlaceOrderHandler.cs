using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{
#pragma warning disable CS0162 // Unreachable code detected

    #region ThrowSystemic
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        // This is normally where some business logic would occur

        throw new Exception("BOOM");

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        return context.Publish(orderPlaced);
    }


    #endregion
#pragma warning restore CS0162 // Unreachable code detected

#pragma warning restore CS0162 // Unreachable code detected

    public void ThrowTransient()
    {
        #region ThrowTransient
        if (Random.Shared.Next(0, 5) == 0)
        {
            throw new Exception("Oops");
        }
        #endregion
    }
}