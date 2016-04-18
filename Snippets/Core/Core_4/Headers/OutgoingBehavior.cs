namespace Core4.Headers
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region header-outgoing-behavior
    public class OutgoingBehavior : IBehavior<SendPhysicalMessageContext>
    {
        public void Invoke(SendPhysicalMessageContext context, Action next)
        {
            Dictionary<string, string> headers = context.MessageToSend.Headers;
            headers["MyCustomHeader"] = "My custom value";
            next();
        }
    }
    #endregion
}
