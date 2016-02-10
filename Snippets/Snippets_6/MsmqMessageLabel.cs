namespace Snippets6
{
    using NServiceBus;

    public class MsmqMessageLabel
    {
        public MsmqMessageLabel()
        {
            #region ApplyLabelToMessages

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MsmqTransport>()
                //Set the msmq message label to the current Message Id
                .ApplyLabelToMessages(headers => headers[NServiceBus.Headers.MessageId]);

            #endregion
        }

    }
}