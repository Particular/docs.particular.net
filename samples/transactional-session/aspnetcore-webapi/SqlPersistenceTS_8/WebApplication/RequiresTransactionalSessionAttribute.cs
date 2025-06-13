using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NServiceBus.TransactionalSession;

#region txsession-filter-attribute

public sealed class RequiresTransactionalSessionAttribute() : TypeFilterAttribute(typeof(TransactionalSessionFilter))
{
    class TransactionalSessionFilter(ITransactionalSession transactionalSession) : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            await transactionalSession.Open(new SqlPersistenceOpenSessionOptions());

            var result = await next();

            if (result.Exception is null)
            {
                await transactionalSession.Commit();
            }
        }
    }
}

#endregion
