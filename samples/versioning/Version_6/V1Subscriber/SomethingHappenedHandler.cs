using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using V1.Messages;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        log.InfoFormat("Something happened with some data {0} and no more info", message.SomeData);
        return Task.FromResult(0);
    }
}