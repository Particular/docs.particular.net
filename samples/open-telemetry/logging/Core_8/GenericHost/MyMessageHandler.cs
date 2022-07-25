using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    private readonly ILogger<MyMessageHandler> logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region log-statement

        logger.LogInformation($"Received message #{message.Number}");

        #endregion

        return Task.CompletedTask;
    }
}