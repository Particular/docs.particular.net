namespace CloudServicesHost_6
{
    using NServiceBus;
    public class EndpointNameInCode : IConfigureThisEndpoint
    {
        #region EndpointNameInCodeForAzureHost
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.EndpointName("CustomEndpointName");
        }
        #endregion
    }
}