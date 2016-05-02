using NServiceBus;

class ScanningPublicApi
{

    #region ScanningConfigurationInNSBHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // use 'busConfiguration' object to configure scanning
        }
    }

    #endregion

}