using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class MyUowBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    readonly MySessionProvider sessionProvider;

    public MyUowBehavior(MySessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var tennant = context.MessageHeaders["tennant"];

        using (var session = await sessionProvider.Open(tennant))
        {
            context.Extensions.Set<IMySession>(session);

            try
            {
                await next();

                await session.Commit();

                Console.Out.WriteLine($"{context.MessageId}: UOW {session.GetHashCode()} was committed");
            }
            catch (Exception)
            {
                await session.Rollback();

                Console.Out.WriteLine($"{context.MessageId}: UOW {session.GetHashCode()} was rolled back");
                throw;
            }
        }
    }
}