using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ServiceControl.RabbitMQAdapter.Adapter";
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<RabbitMQTransport, RabbitMQTransport>("ServiceControl.RabbitMQ.Adapter");

        transportAdapterConfig.EndpointSideErrorQueue = "adapter_error";
        transportAdapterConfig.EndpointSideAuditQueue = "adapter_audit";
        transportAdapterConfig.EndpointSideControlQueue = "adapter_Particular.ServiceControl";

        transportAdapterConfig.ServiceControlSideErrorQueue = "error";
        transportAdapterConfig.ServiceControlSideAuditQueue = "audit";
        transportAdapterConfig.ServiceControlSideControlQueue = "Particular.ServiceControl";

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {
                transport.ConnectionString("host=localhost");
                transport.UseDirectRoutingTopology();
            });

        #endregion

#pragma warning restore 618

        #region SCSideConfig

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                transport.ConnectionString("host=localhost");
            });

        #endregion

        var adapter = TransportAdapter.Create(transportAdapterConfig);

        await adapter.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to shutdown adapter.");
        Console.ReadLine();

        await adapter.Stop()
            .ConfigureAwait(false);
    }
}