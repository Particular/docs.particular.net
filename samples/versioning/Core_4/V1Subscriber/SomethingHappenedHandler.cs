using NServiceBus;
using NServiceBus.Logging;
using V1.Messages;

public class SomethingHappenedHandler :
    IHandleMessages<ISomethingHappened>
{
    static ILog log = LogManager.GetLogger(typeof(SomethingHappenedHandler));

    public void Handle(ISomethingHappened message)
    {
        log.Info($"Something happened with some data {message.SomeData} and no more info");
    }
}