using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class ExpressMessagesHandler : IHandleMessages<RequestExpress>
{
    static ILog log = LogManager.GetLogger<ExpressMessagesHandler>();

    public Task Handle(RequestExpress message, IMessageHandlerContext context)
    {
        log.InfoFormat($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
        return Task.FromResult(0);
    }

}