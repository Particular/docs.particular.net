namespace Core6.Headers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region header-incoming-behavior
    public class IncomingBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            Dictionary<string, string> headers = context.Message.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
            await next().ConfigureAwait(false);
        }
    }
    #endregion
}
