namespace Core8.Mutators.Transport
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateIncomingTransportMessages
    public class MutateIncomingTransportMessages :
        IMutateIncomingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            // the bytes of the incoming messages.
            var bytes = context.Body;

            // optionally replace the Body
            context.Body = ServiceThatChangesBody.Mutate(context.Body);

            // the incoming headers
            var headers = context.Headers;

            // optional manipulate headers

            // add a header
            headers.Add("MyHeaderKey1", "MyHeaderValue");

            // remove a header
            headers.Remove("MyHeaderKey2");

            return Task.CompletedTask;
        }
    }
    #endregion
}
