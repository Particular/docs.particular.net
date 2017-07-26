using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

#region event-handler

class SomethingHappenedHandler :
    IHandleMessages<SomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();
    public Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        log.Info("Subscriber has received SomethingHappened event.");
        return Task.CompletedTask;
    }
}

#endregion