using System;
using NServiceBus.Settings;
using NServiceBus.Transport;

namespace FileBasedRouting_1
{
    class MyTransport :
        TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}
