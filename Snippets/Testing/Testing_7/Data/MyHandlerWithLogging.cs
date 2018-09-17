using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandlerWithLogging :
    IHandleMessages<MyRequest>
{
    public Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        logger.Debug("Some log message");

        return Task.CompletedTask;
    }

    static ILog logger = LogManager.GetLogger<MyHandlerWithLogging>();
}