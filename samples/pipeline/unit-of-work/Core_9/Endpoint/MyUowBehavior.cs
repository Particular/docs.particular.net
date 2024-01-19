using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region unit-of-work-behavior
class MyUowBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    MySessionProvider sessionProvider;

    public MyUowBehavior(MySessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        using (var session = await sessionProvider.Open().ConfigureAwait(false))
        {
            context.Extensions.Set<IMySession>(session);

            try
            {
                await next().ConfigureAwait(false);

                await session.Commit().ConfigureAwait(false);
            }
            catch (Exception)
            {
                await session.Rollback().ConfigureAwait(false);

                throw;
            }
        }
    }
}
#endregion