using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    private static readonly ILogger<OrderAcceptedHandler> logger =
     LoggerFactory.Create(builder =>
     {
         builder.AddConsole();
     }).CreateLogger<OrderAcceptedHandler>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order {message.OrderId} accepted.");
        return Task.CompletedTask;
    }
}