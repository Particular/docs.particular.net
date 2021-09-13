using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region replyHandler
public class RequestHandler :
    IHandleMessages<Request>
{
    static ILog log = LogManager.GetLogger<RequestHandler>();

    public Task Handle(Request message, IMessageHandlerContext context)
    {
        log.Warn("Got Request. Will send Reply");
        var headers = context.MessageHeaders;
        var reply = new Reply
        {
            TheId = message.TheId,
            OriginatingSagaType = headers["NServiceBus.OriginatingSagaType"]
        };
        return context.Reply(reply);
    }
}
#endregion