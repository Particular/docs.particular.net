using System;
using System.Threading.Tasks;
using NServiceBus.Persistence;
using NServiceBus.Pipeline;

#region SetupBehavior

public class UnitOfWorkSetupBehaviorBehavior
    : Behavior<IIncomingLogicalMessageContext>
{
    Func<SynchronizedStorageSession, ReceiverDataContext> contextFactory;

    public UnitOfWorkSetupBehaviorBehavior(Func<SynchronizedStorageSession, ReceiverDataContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public override async Task Invoke(
        IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var uow = new EntityFrameworkUnitOfWork(contextFactory);
        context.Extensions.Set(uow);
        await next().ConfigureAwait(false);
        context.Extensions.Remove<EntityFrameworkUnitOfWork>();
    }
}

#endregion