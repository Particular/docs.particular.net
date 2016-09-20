using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    static ILog log = LogManager.GetLogger<SomeMessageHandler>();
    
    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Interlocked.Increment(ref Program.Counter);

        return Task.FromResult(0);
    }
}