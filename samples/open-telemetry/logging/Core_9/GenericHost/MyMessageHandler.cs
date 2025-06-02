using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyMessageHandler(ILogger<MyMessageHandler> logger) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region log-statement

        logger.LogInformation("Received message #{Number}", message.Number);

        #endregion

        return Task.CompletedTask;
    }
}
