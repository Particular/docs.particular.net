using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

sealed class MyMessageHandler(ILogger<MyMessageHandler> logger) :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing message {MessageId}", message.Id);
        return FailureSimulator.Invoke();
    }
}