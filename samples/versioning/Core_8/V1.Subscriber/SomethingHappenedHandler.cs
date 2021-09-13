using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Versioning.Contracts;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        log.Info($"Something happened with some data {message.SomeData} and no more info");
        return Task.CompletedTask;
    }
}