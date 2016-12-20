namespace MyServer
{
    using NServiceBus;

    public class EndpointNameInCode :
        IConfigureThisEndpoint
    {
        #region EndpointNameInCodeForHost
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.DefineEndpointName("CustomEndpointName");
        }
        #endregion
    }
}