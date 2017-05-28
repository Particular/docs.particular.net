using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region SetupBehavior

public class UnitOfWorkSetupBehaviorBehavior
    : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(
        IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var uow = new EntityFrameworkUnitOfWork();
        context.Extensions.Set(uow);
        await next().ConfigureAwait(false);
        context.Extensions.Remove<EntityFrameworkUnitOfWork>();
    }
}

#endregion