using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class OrderReceivedHandler :
    IHandleMessages<OrderReceived>
{
    private static readonly ILogger<OrderReceivedHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<OrderReceivedHandler>();

    public Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Subscriber has received OrderReceived event with OrderId {message.OrderId}.");
        return Task.CompletedTask;
    }
}