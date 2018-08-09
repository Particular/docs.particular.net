using NServiceBus;
using Serilog;

public class CreateUserHandler :
    IHandleMessages<CreateUser>
{
    static ILogger log = Log.ForContext<CreateUserHandler>();

    public void Handle(CreateUser message)
    {
        log.Information("Hello from CreateUserHandler");
    }
}