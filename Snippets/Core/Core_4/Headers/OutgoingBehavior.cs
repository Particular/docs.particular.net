#pragma warning disable 618
namespace Core4.Headers
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region header-outgoing-behavior
    public class OutgoingBehavior :
        IBehavior<SendPhysicalMessageContext>
    {
        public void Invoke(SendPhysicalMessageContext context, Action next)
        {
            var headers = context.MessageToSend.Headers;
            headers["MyCustomHeader"] = "My custom value";
            next();
        }
    }
    #endregion
}
