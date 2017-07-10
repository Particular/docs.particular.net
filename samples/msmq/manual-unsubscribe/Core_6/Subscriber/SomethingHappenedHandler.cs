using Messages;
using NServiceBus;
using System.Threading.Tasks;

#region event-handler

class SomethingHappenedHandler :
    IHandleMessages<SomethingHappened>
{
    public Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}

#endregion