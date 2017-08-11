using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;
using NServiceBus.Settings;
using NServiceBus.Transport;

public class API
{
    public void Connector()
    {
        #region connector

        var config = new EndpointConfiguration("MyEndpoint");
        var routing = config.UseTransport<MsmqTransport>().Routing();
        var bridge = routing.ConnectToBridge("LeftBank");

        bridge.RouteToEndpoint(typeof(MyMessage), "Receiver");
        bridge.RegisterPublisher(typeof(MyEvent), "Publisher");

        #endregion
    }

    public async Task BridgeConfig()
    {
        #region bridge

        var config = Bridge
            .Between<MsmqTransport>("LeftBank")
            .And<RabbitMQTransport>("RightBank", t => t.ConnectionString("host=localhost"));

        #endregion

        #region lifecycle

        var bridge = config.Create();

        await bridge.Start().ConfigureAwait(false);

        await bridge.Stop().ConfigureAwait(false);

        #endregion
    }

    public class RabbitMQTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    public class MyEvent
    {
    }

    public class MyMessage
    {
    }
}
