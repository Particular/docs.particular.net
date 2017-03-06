using NServiceBus;
using NServiceBus.Logging;

public class CreateUserHandler :
    IHandleMessages<CreateUser>
{
    static ILog log = LogManager.GetLogger(typeof(CreateUserHandler));

    public void Handle(CreateUser message)
    {
        log.Info("Hello from CreateUserHandler");
    }
}