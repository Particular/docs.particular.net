using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using Versioning.Contracts;

public class DoSomethingHandler : IHandleMessages<DoSomething>
{
    static ILog log = LogManager.GetLogger<DoSomethingHandler>();
    public Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        log.Info($"Hi, I did something for you based on your data: {message}");
        return Task.CompletedTask;
    }
}