namespace Core5.UpgradeGuides._5to6
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region 5to6header-outgoing-behavior
    public class OutgoingBehavior : IBehavior<OutgoingContext>
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
