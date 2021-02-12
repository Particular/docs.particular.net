using NServiceBus;

class DisableInstaller
{
    DisableInstaller(EndpointConfiguration endpointConfiguration)
    {
        #region disable-installer

        var transport = new MsmqTransport
        {
            CreateQueues = false
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
