using log4net;
using NServiceBus;
using V1.Messages;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger(typeof(SomethingHappenedHandler));

    public void Handle(ISomethingHappened message)
    {
        log.InfoFormat("Something happened with some data {0} and no more info", message.SomeData);
    }
}