using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class RequestMessageHandler : IHandleMessages<Request>
{
    static readonly ILog log = LogManager.GetLogger<RequestMessageHandler>();

    public Task Handle(Request message, IMessageHandlerContext context)
    {
        log.Info($"Request received with id:{message.RequestId}");

        var response = new Response
        {
            ResponseId = message.RequestId
        };
        return context.Reply(response);
    }
}