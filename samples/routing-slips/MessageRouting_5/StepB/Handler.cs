using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler :
    IHandleMessages<SequentialProcess>
{
    static ILog log = LogManager.GetLogger(typeof(Handler));

    public Task Handle(SequentialProcess message, IMessageHandlerContext context)
    {
        log.Info(message.StepBInfo);

        return Task.CompletedTask;
    }
}