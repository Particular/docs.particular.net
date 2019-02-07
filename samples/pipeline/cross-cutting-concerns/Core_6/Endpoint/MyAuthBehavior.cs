using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region auth-behavior
class MyAuthBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var login = context.MessageHeaders["auth_login"];
        var token = context.MessageHeaders["auth_token"];

        var authContext = new MyAuthContext(login, token);
        context.Extensions.Set(authContext);

        await next().ConfigureAwait(false);
    }
}
#endregion