using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SomeCommandHandler :
    IHandleMessages<SomeCommand>
{
    static ILog log = LogManager.GetLogger<SomeCommandHandler>();

    public Task Handle(SomeCommand message, IMessageHandlerContext context)
    {
        log.Info("Hello from SomeCommandHandler");
        return Task.CompletedTask;
    }
}