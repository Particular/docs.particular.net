namespace MyServer
{
    using NServiceBus;

#pragma warning disable 618
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
#pragma warning restore 618
}