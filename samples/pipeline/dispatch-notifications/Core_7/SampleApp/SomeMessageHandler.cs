using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    static readonly ILog Log = LogManager.GetLogger<SomeMessageHandler>();

    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Log.Info("Got SomeMessage");
        return Task.CompletedTask;
    }
}