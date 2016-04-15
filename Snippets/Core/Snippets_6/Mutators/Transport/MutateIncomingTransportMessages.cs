namespace Core6.Mutators.Transport
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateIncomingTransportMessages
    public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            // the bytes containing the incoming messages.
            byte[] bytes = context.Body;

            // optionally replace the Body
            context.Body = ServiceThatChangesBody.Mutate(context.Body);


            // the incoming headers
            IDictionary<string, string> headers = context.Headers;

            // optional manipulate headers

            // add a header
            headers.Add("MyHeaderKey1", "MyHeaderValue");

            // remove a header
            headers.Remove("MyHeaderKey2");

            return Task.FromResult(0);
        }

    }
    #endregion
}
