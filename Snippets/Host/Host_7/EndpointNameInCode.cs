namespace MyServer
{
    using NServiceBus;

    public class EndpointNameInCode : IConfigureThisEndpoint
    {
        #region EndpointNameInCodeForHost
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.DefineEndpointName("CustomEndpointName");
        }
        #endregion
    }
}