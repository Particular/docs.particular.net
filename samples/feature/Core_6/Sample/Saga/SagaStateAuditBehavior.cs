using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;

#region SagaStateAuditBehavior
class SagaStateAuditBehavior :
    Behavior<IInvokeHandlerContext>
{
    CustomLogger logger;

    public SagaStateAuditBehavior(CustomLogger logger)
    {
        this.logger = logger;
    }

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        await next()
            .ConfigureAwait(false);
        if (context.Extensions.TryGet(out var activeSagaInstance))
        {
            var instance = activeSagaInstance.Instance.Entity;
            logger.WriteSaga(instance);
        }
    }
}
#endregion


