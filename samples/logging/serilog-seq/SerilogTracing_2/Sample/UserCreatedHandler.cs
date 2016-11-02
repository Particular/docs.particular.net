using NServiceBus;
using NServiceBus.Logging;

public class UserCreatedHandler :
    IHandleMessages<UserCreated>
{
    static ILog logger = LogManager.GetLogger(typeof(UserCreatedHandler));

    public void Handle(UserCreated message)
    {
        logger.Info("Hello from UserCreatedHandler");
    }
}