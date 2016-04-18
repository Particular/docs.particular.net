namespace Core5.Headers
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region header-outgoing-behavior
    public class OutgoingBehavior : IBehavior<OutgoingContext>
    {
        public void Invoke(OutgoingContext context, Action next)
        {
            Dictionary<string, string> headers = context.OutgoingMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
            next();
        }
    }
    #endregion
}
