using System.Threading.Tasks;
using NServiceBus;
using Serilog;

public class UserCreatedHandler :
    IHandleMessages<UserCreated>
{
    static ILogger log = Log.ForContext<UserCreatedHandler>();

    public Task Handle(UserCreated message, IMessageHandlerContext context)
    {
        log.Information("Hello from {@Handler}", nameof(UserCreatedHandler));
        return Task.CompletedTask;
    }
}