namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;
    using NServiceBus.TransportDispatch;

    #region 5to6header-outgoing-behavior
    public class OutgoingBehavior : Behavior<OutgoingContext>
    {
        public override void Invoke(OutgoingContext context, Action next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            next();
        }
    }
    #endregion
}
