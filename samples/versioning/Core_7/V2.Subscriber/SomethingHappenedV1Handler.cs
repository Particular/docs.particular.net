using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

public class SomethingHappenedV1Handler : IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedV1Handler>();

    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        log.Info("Received a v1 event and missing data, do what's needed to retrieve that data");

        context.Publish<ISomethingMoreHappened>(v2 =>
        {
            v2.SomeData = message.SomeData;
            v2.MoreInfo = "more info"; // set this value with the retrieved data
        }).ConfigureAwait(false);

        return Task.CompletedTask;
    }
}