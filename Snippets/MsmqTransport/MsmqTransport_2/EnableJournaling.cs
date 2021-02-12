using NServiceBus;

class EnableJournaling
{
    EnableJournaling(EndpointConfiguration endpointConfiguration)
    {
        #region enable-journaling

        var transport = new MsmqTransport
        {
            UseJournalQueue = true
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
