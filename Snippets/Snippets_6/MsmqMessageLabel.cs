namespace Snippets6
{
    using NServiceBus;

    class MsmqMessageLabel
    {
        MsmqMessageLabel(EndpointConfiguration endpointConfiguration)
        {
            #region ApplyLabelToMessages
            endpointConfiguration.UseTransport<MsmqTransport>()
                //Set the msmq message label to the current Message Id
                .ApplyLabelToMessages(headers => headers[NServiceBus.Headers.MessageId]);

            #endregion
        }

    }
}