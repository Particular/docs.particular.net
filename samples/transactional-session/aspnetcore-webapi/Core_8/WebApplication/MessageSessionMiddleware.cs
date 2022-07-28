using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NServiceBus.TransactionalSession;

public class MessageSessionMiddleware
{
    readonly RequestDelegate next;

    public MessageSessionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, ITransactionalSession session)
    {
        await session.Open();
        await next(httpContext);
        await session.Commit();
    }
}