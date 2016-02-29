namespace Snippets6
{
    using NServiceBus;

    public class MsmqMessageLabel
    {
        public MsmqMessageLabel()
        {
            #region ApplyLabelToMessages

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<MsmqTransport>()
                //Set the msmq message label to the current Message Id
                .ApplyLabelToMessages(headers => headers[NServiceBus.Headers.MessageId]);

            #endregion
        }

    }
}