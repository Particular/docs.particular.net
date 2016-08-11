namespace MyServer
{
    using NServiceBus;

    public class EndpointNameInCode : IConfigureThisEndpoint
    {
        #region EndpointNameInCode
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.DefineEndpointName("CustomEndpointName");
        }
        #endregion
    }
}