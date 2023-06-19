using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NServiceBus.TransactionalSession;

#region txsession-filter-attribute

public sealed class RequiresTransactionalSessionAttribute : TypeFilterAttribute
{
    public RequiresTransactionalSessionAttribute() : base(typeof(TransactionalSessionFilter))
    {
    }

    private class TransactionalSessionFilter : IAsyncResourceFilter
    {
        public TransactionalSessionFilter(ITransactionalSession transactionalSession)
        {
            this.transactionalSession = transactionalSession;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            await transactionalSession.Open(new SqlPersistenceOpenSessionOptions());

            await next();

            await transactionalSession.Commit();
        }

        private readonly ITransactionalSession transactionalSession;
    }
}

#endregion