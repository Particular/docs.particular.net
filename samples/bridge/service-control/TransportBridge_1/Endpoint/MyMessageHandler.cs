using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

class MyMessageHandler (ILogger<MyMessageHandler> logger):
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing message {Id}", message.Id);
        return FailureSimulator.Invoke();
    }
}