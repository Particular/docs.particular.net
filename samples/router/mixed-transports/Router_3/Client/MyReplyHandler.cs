using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyReplyHandler :
    IHandleMessages<MyReply>
{
    static ILog log = LogManager.GetLogger<MyReplyHandler>();

    public Task Handle(MyReply message, IMessageHandlerContext context)
    {
        log.Info($"Reply {message.Id}");
        return Task.CompletedTask;
    }
}