using NServiceBus;
using NServiceBus.FileBasedRouting;

namespace FileBasedRouting_1
{
    class Snippets
    {
        public void Enable(EndpointConfiguration endpointConfiguration)
        {
            #region Enable

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            var routing = transport.Routing();
            routing.UseFileBasedRouting();

            #endregion
        }

        public void EnableCustomPath(EndpointConfiguration endpointConfiguration)
        {
            #region EnableCustomPath

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            var routing = transport.Routing();
            routing.UseFileBasedRouting(@"C:\routingFile.xml");

            #endregion
        }
    }
}