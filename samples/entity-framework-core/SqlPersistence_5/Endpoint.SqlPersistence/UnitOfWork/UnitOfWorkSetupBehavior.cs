using System;
using System.Threading.Tasks;
using NServiceBus.Persistence;
using NServiceBus.Pipeline;

#region SetupBehavior

public class UnitOfWorkSetupBehavior
    : Behavior<IIncomingLogicalMessageContext>
{
    Func<SynchronizedStorageSession, ReceiverDataContext> contextFactory;

    public UnitOfWorkSetupBehavior(Func<SynchronizedStorageSession, ReceiverDataContext> contextFactory)
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