using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    private readonly ILogger<MyMessageHandler> logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        this.logger = logger;
    }
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Processing message {message.Id}");
        return FailureSimulator.Invoke();
    }
}