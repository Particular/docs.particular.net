#pragma warning disable IDE0059 // Unnecessary assignment of a value
namespace Core8.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region SetContextBetweenIncomingAndOutgoing

    class SharedState { }

    public class SetContextBehavior :
        Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            // set the state
            context.Extensions.Set(new SharedData());

            await next().ConfigureAwait(false);
        }
    }

    public class GetContextBehavior :
        Behavior<IOutgoingPhysicalMessageContext>
    {
        public override async Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {

            if (context.Extensions.TryGet<SharedData>(out var state))
            {
                // work with the state
            }

            await next().ConfigureAwait(false);
        }
    }

    #endregion
}
#pragma warning restore IDE0059 // Unnecessary assignment of a value