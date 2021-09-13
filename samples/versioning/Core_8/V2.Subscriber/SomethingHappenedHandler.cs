using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class SomethingHappenedHandler : IHandleMessages<ISomethingMoreHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(ISomethingMoreHappened message, IMessageHandlerContext context)
    {
        log.Info($"Something happened with some data {message.SomeData} and more information {message.MoreInfo}");
        return Task.CompletedTask;
    }
}