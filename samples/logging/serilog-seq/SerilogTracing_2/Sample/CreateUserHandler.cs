using NServiceBus;
using NServiceBus.Logging;

public class CreateUserHandler :
    IHandleMessages<CreateUser>
{
    static ILog logger = LogManager.GetLogger(typeof(CreateUserHandler));

    public void Handle(CreateUser message)
    {
        logger.Info("Hello from CreateUserHandler");
    }
}