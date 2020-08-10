namespace Core8.Headers
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region header-incoming-mutator
    public class MutateIncomingTransportMessages :
        IMutateIncomingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            var headers = context.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
            return Task.CompletedTask;
        }
    }
    #endregion
}
