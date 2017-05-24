namespace Core7.Headers
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region header-incoming-mutator
    public class MutateIncomingTransportMessages :
        IMutateIncomingTransportMessages
    {
        public async Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            var headers = context.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}
