namespace CloudServicesHost_7
{
    using NServiceBus;
    public class EndpointNameInCode : IConfigureThisEndpoint
    {
        #region EndpointNameInCodeForAzureHost
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.DefineEndpointName("CustomEndpointName");
        }
        #endregion
    }
}