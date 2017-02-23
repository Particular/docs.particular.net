using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class ReplyHandler :
    IHandleMessages<ReplyMessage>
{
    static ILog log = LogManager.GetLogger<ReplyHandler>();

    public Task Handle(ReplyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Got reply from {message.SomeId}");
        return context.Reply(new ReplyFollowUpMessage());
    }
}