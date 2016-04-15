namespace Core4.Headers
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region header-incoming-behavior
    public class IncomingBehavior : IBehavior<ReceivePhysicalMessageContext>
    {
        public void Invoke(ReceivePhysicalMessageContext context, Action next)
        {
            Dictionary<string, string> headers = context.PhysicalMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
}
