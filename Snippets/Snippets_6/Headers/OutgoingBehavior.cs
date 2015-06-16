namespace Snippets6.Headers
{
    using System;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region header-outgoing-behavior
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
