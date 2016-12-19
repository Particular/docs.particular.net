using NServiceBus;
using NServiceBus.FileBasedRouting;

namespace FIleBasedRouting_1
{
    class Snippets
    {
        public void Enable(EndpointConfiguration config)
        {
            #region Enable

            var routing = config.UseTransport<MyTransport>().Routing();
            routing.UseFileBasedRouting();

            #endregion
        }

        public void EnableCustomPath(EndpointConfiguration config)
        {
            #region EnableCustomPath

            var routing = config.UseTransport<MyTransport>().Routing();
            routing.UseFileBasedRouting(@"C:\routingFile.xml");

            #endregion
        }
    }
}