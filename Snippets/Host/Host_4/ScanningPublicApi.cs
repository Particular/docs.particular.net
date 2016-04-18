namespace Core4.Scanning
{
    using NServiceBus;

    class ScanningPublicApi
    {
        #region ScanningConfigurationInNSBHost

        public class EndpointConfig : IConfigureThisEndpoint, IWantCustomInitialization
        {
            public void Init()
            {
                // use 'Configure' to configure scanning
            }
        }

        #endregion
    }
}