using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.TransactionalSession;

#region txsession-filter
public class MessageSessionFilter : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (context.ActionDescriptor.Parameters.Any(p => p.ParameterType == typeof(ITransactionalSession)))
        {
            var session = context.HttpContext.RequestServices.GetRequiredService<ITransactionalSession>();
            await session.Open(new SqlPersistenceOpenSessionOptions());

            var result = await next();

            if (result.Exception is null)
            {
                await session.Commit();
            }
        }
        else
        {
            await next();
        }
    }
}
#endregion
