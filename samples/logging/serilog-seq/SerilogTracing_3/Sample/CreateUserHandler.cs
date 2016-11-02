using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class CreateUserHandler :
    IHandleMessages<CreateUser>
{
    static ILog logger = LogManager.GetLogger(typeof(CreateUserHandler));

    public Task Handle(CreateUser message, IMessageHandlerContext context)
    {
        logger.Info("Hello from CreateUserHandler");
        return Task.FromResult(0);
    }
}