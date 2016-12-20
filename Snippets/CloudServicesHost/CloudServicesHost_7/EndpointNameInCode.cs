namespace CloudServicesHost_7
{
    using NServiceBus;
    public class EndpointNameInCode :
        IConfigureThisEndpoint
    {
        #region EndpointNameInCodeForAzureHost
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.DefineEndpointName("CustomEndpointName");
        }
        #endregion
    }
}