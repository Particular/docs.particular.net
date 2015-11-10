namespace Snippets5.UpgradeGuides._5to6
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region 5to6header-outgoing-behavior
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
