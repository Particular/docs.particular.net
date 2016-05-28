namespace Core6.UpgradeGuides._5to6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region 5to6header-outgoing-behavior
    public class OutgoingBehavior : Behavior<IOutgoingLogicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            context.Headers["MyCustomHeader"] = "My custom value";
            return next();
        }
    }
    #endregion
}
