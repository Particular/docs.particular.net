using System.Threading.Tasks;
using Core_9.Messages;
using NServiceBus;
using Microsoft.Extensions.Logging;

namespace Core_9;

#region PlaceOrderHandler

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation(
            "Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        return Task.CompletedTask;
    }
}

#endregion