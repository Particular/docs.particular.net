namespace Snippets3.EndpointName
{
    // startcode EndpointNameByNamespace
    namespace MyServer
    {
        using NServiceBus;

        public class EndpointConfigByNamespace : IConfigureThisEndpoint, AsA_Server
        {
            // ... custom config
            // endcode
        }
    }
}
