using NServiceBus;

class ScanningPublicApi
{

    #region ScanningConfigurationInNSBHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // use 'busConfiguration' object to configure scanning
        }
    }

    #endregion
}