using NServiceBus;

class ScanningPublicApi
{

#pragma warning disable 618
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
#pragma warning restore 618
}