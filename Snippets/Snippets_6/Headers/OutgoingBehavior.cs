namespace Snippets6.Headers
{
    using System;
    using NServiceBus.OutgoingPipeline;
    using NServiceBus.TransportDispatch;

    #region header-outgoing-behavior
    public class OutgoingBehavior : PhysicalOutgoingContextStageBehavior
    {
        public override void Invoke(Context context, Action next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            next();
        }
    }
    #endregion
}