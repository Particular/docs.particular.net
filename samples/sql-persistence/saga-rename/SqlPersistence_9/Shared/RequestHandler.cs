using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

namespace Shared;

#region replyHandler
public class RequestHandler(ILogger<RequestHandler> logger) : IHandleMessages<Request>
{
    public Task Handle(Request message, IMessageHandlerContext context)
    {
        logger.LogInformation("Got request {requestId}", message.TheId);

        var headers = context.MessageHeaders;

        var reply = new Reply
        {
            TheId = message.TheId,
            OriginatingSagaType = headers["NServiceBus.OriginatingSagaType"]
        };

        logger.LogInformation("Sending reply {replyId} to {originatingSaga}", reply.TheId, reply.OriginatingSagaType);

        return context.Reply(reply);
    }
}
#endregion