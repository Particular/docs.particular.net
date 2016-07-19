using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Saga;

#region SagaStateAuditBehavior
class SagaStateAuditBehavior :
    IBehavior<IncomingContext>
{
    CustomLogger logger;

    public SagaStateAuditBehavior(CustomLogger logger)
    {
        this.logger = logger;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        next();
        var saga = context.MessageHandler.Instance as Saga;
        if (saga != null)
        {
            var instance = saga.Entity;
            logger.WriteSaga(instance);
        }
    }
}
#endregion


