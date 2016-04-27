namespace Core6
{
    using NServiceBus;

    class MsmqMessageLabel
    {
        MsmqMessageLabel(EndpointConfiguration endpointConfiguration)
        {
            #region ApplyLabelToMessages

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            //Set the msmq message label to the current Message Id
            transport.ApplyLabelToMessages(headers => headers[NServiceBus.Headers.MessageId]);

            #endregion
        }
    }
}