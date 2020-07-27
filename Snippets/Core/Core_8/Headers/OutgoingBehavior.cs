namespace Core8.Headers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region header-outgoing-behavior
    public class OutgoingBehavior :
        Behavior<IOutgoingPhysicalMessageContext>
    {
        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            var headers = context.Headers;
            headers["MyCustomHeader"] = "My custom value";
            return next();
        }
    }
    #endregion
}