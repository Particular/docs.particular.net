using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region sqlrelay-handler
class SomethingHappenedHandler :
    IHandleMessages<SomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        log.Info("Sql Relay has now received this event from the MSMQ. Publishing this event for downstream SQLSubscribers");
        // relay this event to other interested SQL subscribers
        return context.Publish(message);
    }
}
#endregion