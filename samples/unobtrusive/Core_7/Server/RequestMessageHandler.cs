using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class RequestMessageHandler :
    IHandleMessages<Request>
{
    static ILog log = LogManager.GetLogger<RequestMessageHandler>();

    public Task Handle(Request message, IMessageHandlerContext context)
    {
        log.Info($"Request received with id:{message.RequestId}");

        var response = new Response
        {
            ResponseId = message.RequestId
        };
        context.Reply(response);
        return Task.CompletedTask;
    }
}