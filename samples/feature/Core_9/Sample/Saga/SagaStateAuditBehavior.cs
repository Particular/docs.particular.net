using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;

#region SagaStateAuditBehavior
class SagaStateAuditBehavior(CustomLogger logger) :
    Behavior<IInvokeHandlerContext>
{
    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        await next();
        if (context.Extensions.TryGet(out ActiveSagaInstance activeSagaInstance))
        {
            var instance = activeSagaInstance.Instance.Entity;
            logger.WriteSaga(instance);
        }
    }
}
#endregion


