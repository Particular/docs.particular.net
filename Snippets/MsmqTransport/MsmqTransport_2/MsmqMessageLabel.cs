namespace Core6
{
    using NServiceBus;

    class MsmqMessageLabel
    {
        MsmqMessageLabel(EndpointConfiguration endpointConfiguration)
        {
            #region ApplyLabelToMessages

            var transport = new MsmqTransport
            {
                // Set the msmq message label to the current Message Id
                ApplyCustomLabelToOutgoingMessages = headers => headers[Headers.MessageId]
            };
            endpointConfiguration.UseTransport(transport);

            #endregion
        }
    }
}