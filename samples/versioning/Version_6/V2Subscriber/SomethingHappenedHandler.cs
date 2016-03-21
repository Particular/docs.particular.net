using System.Threading.Tasks;
using V2.Messages;
using NServiceBus;
using NServiceBus.Logging;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        log.InfoFormat("Something happened with some data {0} and more information {1}", message.SomeData, message.MoreInfo);
        return Task.FromResult(0);
    }

}