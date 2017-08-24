using NServiceBus;

class ScanningPublicApi
{

    #region ScanningConfigurationInNSBHost

    public class EndpointConfig :
        IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // use 'endpointConfiguration' object to configure scanning
        }
    }

    #endregion
}