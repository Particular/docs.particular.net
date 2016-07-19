namespace Core5.Headers
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region header-outgoing-behavior
    public class OutgoingBehavior :
        IBehavior<OutgoingContext>
    {
        public void Invoke(OutgoingContext context, Action next)
        {
            var headers = context.OutgoingMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
            next();
        }
    }
    #endregion
}
