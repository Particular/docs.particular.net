using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class UserCreatedHandler :
    IHandleMessages<UserCreated>
{
    static ILog log = LogManager.GetLogger(typeof(UserCreatedHandler));

    public Task Handle(UserCreated message, IMessageHandlerContext context)
    {
        log.InfoFormat("Hello from {@Handler}", nameof(UserCreatedHandler));
        return Task.FromResult(0);
    }
}