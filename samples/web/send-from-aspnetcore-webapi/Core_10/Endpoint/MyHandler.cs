using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyHandler(ILogger<MyHandler> logger) :
    IHandleMessages<MyMessage>
{
    #region MessageHandler
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received at endpoint");
        return Task.CompletedTask;
    }
    #endregion
}