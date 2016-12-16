using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.FileBasedRouting;
using NServiceBus.Settings;
using NServiceBus.Transport;

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

    class MyTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}
