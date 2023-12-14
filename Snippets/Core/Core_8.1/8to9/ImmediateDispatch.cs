using NServiceBus;
using NServiceBus.Pipeline;
using System;
using System.Threading.Tasks;

class ImmediateDispatch
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-immediate-dispatch-old
    class MyOldBehavior : Behavior<IOutgoingContext>
    {
        public override Task Invoke(IOutgoingContext context, Func<Task> next)
        {
            var sendOptions = context.Extensions.Get<SendOptions>();

            if (sendOptions.RequiredImmediateDispatch())
            {
                // do something
            }

            return next();
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete

    #region core-8to9-immediate-dispatch-new
    class MyNewBehavior : Behavior<IOutgoingContext>
    {
        public override Task Invoke(IOutgoingContext context, Func<Task> next)
        {
            var sendOptions = context.Extensions.Get<SendOptions>();

            if (sendOptions.IsImmediateDispatchSet())
            {
                // do something
            }

            return next();
        }
    }
    #endregion
}

