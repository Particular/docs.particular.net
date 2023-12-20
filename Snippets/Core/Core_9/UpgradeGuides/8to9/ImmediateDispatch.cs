using NServiceBus;
using NServiceBus.Pipeline;
using System;
using System.Threading.Tasks;

class ImmediateDispatch
{
    #region core-8to9-immediate-dispatch
    class MyBehavior : Behavior<IOutgoingContext>
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