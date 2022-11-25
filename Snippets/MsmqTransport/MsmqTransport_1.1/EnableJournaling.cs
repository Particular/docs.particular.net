using NServiceBus;

class EnableJournaling
{
    EnableJournaling(EndpointConfiguration endpointConfiguration)
    {
        #region enable-journaling

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.EnableJournaling();

        #endregion
    }
}
