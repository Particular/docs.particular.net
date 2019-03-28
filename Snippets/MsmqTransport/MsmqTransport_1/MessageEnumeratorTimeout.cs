using NServiceBus;

class MessageEnumeratorTimeout
{
    MessageEnumeratorTimeout(EndpointConfiguration endpointConfiguration)
    {
        #region message-enumerator-timeout

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.MessageEnumeratorTimeout(TimeSpan.FromSeconds(2));

        #endregion
    }
}


