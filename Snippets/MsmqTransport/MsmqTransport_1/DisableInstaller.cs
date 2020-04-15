using NServiceBus;

class DisableInstaller
{
    DisableInstaller(EndpointConfiguration endpointConfiguration)
    {
        #region disable-installer

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DisableInstaller();
        #endregion
    }
}
