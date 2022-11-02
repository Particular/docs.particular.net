using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        #region RequestHandler

        log.Info($"Command {message.Id}");
        
        #endregion

        return Task.CompletedTask;
    }
}