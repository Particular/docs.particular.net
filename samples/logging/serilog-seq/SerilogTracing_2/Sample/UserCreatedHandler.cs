using NServiceBus;
using Serilog;

public class UserCreatedHandler :
    IHandleMessages<UserCreated>
{
    static ILogger log = Log.ForContext<UserCreatedHandler>();

    public void Handle(UserCreated message)
    {
        log.Information("Hello from UserCreatedHandler");
    }
}