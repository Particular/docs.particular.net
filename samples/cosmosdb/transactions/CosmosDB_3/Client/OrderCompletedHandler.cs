using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderCompletedHandler :
    IHandleMessages<OrderCompleted>
{
    private static readonly ILogger<OrderCompletedHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<OrderCompletedHandler>();

    public Task Handle(OrderCompleted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received OrderCompleted for OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}