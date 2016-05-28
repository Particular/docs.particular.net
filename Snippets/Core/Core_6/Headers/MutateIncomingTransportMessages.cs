namespace Core6.Headers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region header-incoming-mutator
    public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
    {
        public async Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            IDictionary<string, string> headers = context.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}
