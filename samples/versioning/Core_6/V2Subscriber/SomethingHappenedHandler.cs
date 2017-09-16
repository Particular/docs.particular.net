using System.Threading.Tasks;
using V2.Messages;
using NServiceBus;
using NServiceBus.Logging;

public class SomethingHappenedHandler :
    IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        log.Info($"Something happened with some data {message.SomeData} and more information {message.MoreInfo}");
        return Task.CompletedTask;
    }
}