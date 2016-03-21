using V2.Messages;
using NServiceBus;
using NServiceBus.Logging;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger(typeof(SomethingHappenedHandler));

    public void Handle(ISomethingHappened message)
    {
        log.InfoFormat("Something happened with some data {0} and more information {1}", message.SomeData, message.MoreInfo);
    }
}