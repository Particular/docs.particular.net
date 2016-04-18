namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region SharingBehaviorData

    public class ParentBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            // set some shared information on the context
            context.Extensions.Set(new SharedData());

            await next().ConfigureAwait(false);
        }
    }

    public class ChildBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            // access the shared data
            SharedData data = context.Extensions.Get<SharedData>();

            await next().ConfigureAwait(false);
        }
    }

#endregion

    public class SharedData
    {
    }
}