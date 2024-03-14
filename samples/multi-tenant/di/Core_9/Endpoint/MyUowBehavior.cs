using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region unit-of-work-behavior
class MyUowBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var tenant = context.MessageHeaders["tenant"];

        var session = context.Builder.GetService(typeof(MySession)) as MySession;
        session.Initialize(tenant);

        try
        {
            await next();

            await session.Commit();
        }
        catch (Exception)
        {
            await session.Rollback();

            throw;
        }
    }
}
#endregion