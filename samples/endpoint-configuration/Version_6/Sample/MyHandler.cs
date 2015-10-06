using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    public Task Handle(MyMessage message)
    {
        logger.Info("Hello from MyHandler");
        return Task.FromResult(0);
    }
}