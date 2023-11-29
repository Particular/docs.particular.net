using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class ResponseHandler :
    IHandleMessages<Response>
{
    static ILog log = LogManager.GetLogger<ResponseHandler>();

    public Task Handle(Response message, IMessageHandlerContext context)
    {
        log.Info($"Response received from server for request with id:{message.ResponseId}");
        return Task.CompletedTask;
    }
}