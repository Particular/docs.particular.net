using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region handler-using-custom-header

public class HandlerUsingAccessor :
    IHandleMessages<MyMessage>
{
    readonly IPrincipalAccessor principalAccessor;
    private readonly ILogger<HandlerUsingAccessor> logger;

    public HandlerUsingAccessor(IPrincipalAccessor principalAccessor, ILogger<HandlerUsingAccessor> logger)
    {
        this.principalAccessor = principalAccessor;
        this.logger = logger;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var headers = context.MessageHeaders;
        var usernameFromHeader = headers["UserName"];
        var usernameFromAccessor = principalAccessor?.CurrentPrincipal?.Identity?.Name ?? "null";
        logger.LogInformation("Username extracted from header: {UsernameFromHeader}", usernameFromHeader);
        logger.LogInformation("Username extracted from accessor: {UsernameFromAccessor}", usernameFromAccessor);
        return Task.CompletedTask;
    }
}

#endregion