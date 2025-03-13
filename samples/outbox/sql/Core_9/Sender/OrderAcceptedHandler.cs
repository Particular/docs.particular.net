using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    private readonly ILogger<OrderAcceptedHandler> logger;
    public OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order {message.OrderId} accepted.");
        return Task.CompletedTask;
    }
}