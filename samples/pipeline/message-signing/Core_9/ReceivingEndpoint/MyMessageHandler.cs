using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class MyMessageHandler(ILogger<MyMessageHandler> logger) :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var signature = context.MessageHeaders["X-Message-Signature"];

        logger.LogInformation("Handling message...");
        logger.LogInformation("  Contents = {Contents}", message.Contents);
        logger.LogInformation("  Signature = {Signature}", signature);

        return Task.CompletedTask;
    }
}