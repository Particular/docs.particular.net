using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {

        logger.LogInformation($"PlaceOrder command received: {message.OrderId} {message.Value}");
        return Task.CompletedTask;
    }
}