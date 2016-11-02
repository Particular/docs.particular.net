using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class UserCreatedHandler :
    IHandleMessages<UserCreated>
{
    static ILog logger = LogManager.GetLogger(typeof(UserCreatedHandler));

    public Task Handle(UserCreated message, IMessageHandlerContext context)
    {
        logger.Info("Hello from UserCreatedHandler");
        return Task.FromResult(0);
    }
}