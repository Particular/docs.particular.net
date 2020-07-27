namespace Core8.Headers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region header-incoming-behavior
    public class IncomingBehavior :
        Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            var headers = context.Message.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
            return next();
        }
    }
    #endregion
}
