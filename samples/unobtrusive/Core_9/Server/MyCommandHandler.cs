using System.Threading.Tasks;
using Commands;
using NServiceBus;
using NServiceBus.Logging;

public class MyCommandHandler :
    IHandleMessages<MyCommand>
{
    static ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        log.Info($"Command received, id:{message.CommandId}");
        return Task.CompletedTask;
    }
}