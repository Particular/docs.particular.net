using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler-using-custom-header

public class HandlerUsingAccessor :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger("HandlerUsingAccessor");
    readonly IPrincipalAccessor principalAccessor;

    public HandlerUsingAccessor(IPrincipalAccessor principalAccessor)
    {
        this.principalAccessor = principalAccessor;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var headers = context.MessageHeaders;
        var usernameFromHeader = headers["UserName"];
        var usernameFromAccessor = principalAccessor?.CurrentPrincipal?.Identity?.Name ?? "null";
        log.Info($"Username extracted from header: {usernameFromHeader}");
        log.Info($"Username extracted from accessor: {usernameFromAccessor}");
        return Task.CompletedTask;
    }
}

#endregion