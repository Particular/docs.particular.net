using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class RequestMessageHandler
    : IHandleMessages<RequestMessage>
{
    static ILog log = LogManager.GetLogger<RequestMessageHandler>();

    public Task Handle(RequestMessage message, IMessageHandlerContext context)
    {
        log.Info($"Request received with description: {message.Data}");

        var response = new ResponseMessage
        {
            Id = message.Id,
            Data = message.Data
        };
        return context.Reply(response);
    }
}