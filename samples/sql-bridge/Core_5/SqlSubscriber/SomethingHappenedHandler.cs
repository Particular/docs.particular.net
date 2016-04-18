using Shared;
using NServiceBus;
using NServiceBus.Logging;

#region sqlsubscriber-handler
class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public void Handle(SomethingHappened message)
    {
        log.Info("Sql subscriber has now received this event. This was originally published by MSMQ publisher.");
    }
}
#endregion