namespace Core8.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region SharingBehaviorData

    public class ParentBehavior :
        Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            // set some shared information on the context
            context.Extensions.Set(new SharedData());
            return next();
        }
    }

    public class ChildBehavior :
        Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            // access the shared data
            var sharedData = context.Extensions.Get<SharedData>();

            return next();
        }
    }

#endregion

    public class SharedData
    {
    }
}