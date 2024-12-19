using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler-using-custom-header

public class HandlerUsingAccessor(IPrincipalAccessor principalAccessor) :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger(nameof(HandlerUsingAccessor));

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var headers = context.MessageHeaders;
        var usernameFromHeader = headers[Headers.UserName];
        var usernameFromAccessor = principalAccessor?.CurrentPrincipal?.Identity?.Name ?? "null";
        log.InfoFormat("Username extracted from header: {0}", usernameFromHeader);
        log.InfoFormat("Username extracted from accessor: {0}", usernameFromAccessor);
        return Task.CompletedTask;
    }
}

#endregion