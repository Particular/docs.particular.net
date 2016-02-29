namespace Snippets6.EndpointName
{
    using NServiceBus;

    // startcode EndpointNameByAttribute
    [EndpointName("MyEndpointName")]
    public class EndpointConfigWithAttribute : IConfigureThisEndpoint, AsA_Server
    {
        // ... your custom config
        // endcode
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
        }
    }

}
