using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    private static readonly ILogger<MyMessageHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<MyMessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Processing message {message.Id}");
        return FailureSimulator.Invoke();
    }
}