using NServiceBus.Pipeline;

namespace Snippets6.Headers
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region header-incoming-behavior
    public class IncomingBehavior : PhysicalMessageProcessingStageBehavior
    {
        public override void Invoke(Context context, Action next)
        {
            Dictionary<string, string> headers = context.GetPhysicalMessage().Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            next();
        }
    }
    #endregion
}
