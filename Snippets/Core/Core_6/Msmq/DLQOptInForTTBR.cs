namespace Core6.Msmq
{
    using NServiceBus;

    class DLQOptInForTTBR
    {
        DLQOptInForTTBR(EndpointConfiguration endpointConfiguration)
        {
            #region msmq-dlq-for-ttbr-optin

            endpointConfiguration.UseTransport<MsmqTransport>()
                .UseDeadLetterQueueForMessagesWithTimeToReachQueue();

            #endregion
        }
    }
}