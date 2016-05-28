namespace Core5.Pipeline
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region SharingBehaviorData

    public class ParentBehavior : IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            // set some shared information on the context
            context.Set(new SharedData());

            next();
        }
    }

    public class ChildBehavior : IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            // access the shared data
            var data = context.Get<SharedData>();

            next();
        }
    }

    #endregion

    public class SharedData
    {
    }
}