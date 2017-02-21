using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region InitializeBehavior

public class UnitOfWorkInitializeBehavior 
    : Behavior<IInvokeHandlerContext>
{
    public override Task Invoke(
        IInvokeHandlerContext context, Func<Task> next)
    {
        var uow = context.Extensions.Get<EntityFrameworkUnitOfWork>();
        uow.Initialize(context);
        return next();
    }
}

#endregion