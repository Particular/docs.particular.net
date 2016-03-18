namespace Snippets5.EndpointName
{
    // startcode EndpointNameByNamespace
    namespace MyServer
    {
        using NServiceBus;

        public class EndpointConfigByNamespace : IConfigureThisEndpoint, AsA_Server
        {
            // ... custom config
            // endcode
            public void Customize(BusConfiguration busConfiguration)
            {
            }
        }
    }
}
