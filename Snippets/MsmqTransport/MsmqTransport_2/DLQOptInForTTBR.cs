namespace Core6.Msmq
{
    using NServiceBus;

    class DLQOptInForTTBR
    {
        DLQOptInForTTBR(EndpointConfiguration endpointConfiguration)
        {
            #region msmq-dlq-for-ttbr-optin

            var transport = new MsmqTransport
            {
                UseDeadLetterQueueForMessagesWithTimeToBeReceived = true
            };
            endpointConfiguration.UseTransport(transport);

            #endregion
        }
    }
}
