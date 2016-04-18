using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Server-Handler
public class DoSomethingHandler : IHandleMessages<DoSomething>
{
    static ILog log = LogManager.GetLogger<DoSomethingHandler>();

    public Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        log.Info("Message received.");
        return Task.FromResult(0);
    }
}
#endregion
