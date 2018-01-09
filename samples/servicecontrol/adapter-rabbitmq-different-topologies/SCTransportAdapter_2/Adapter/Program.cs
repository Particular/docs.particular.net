using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.RabbitMQAdapter.Adapter";
        #region AdapterTransport

        var transportAdapter =
            new TransportAdapterConfig<RabbitMQTransport, RabbitMQTransport>("ServiceControl.RabbitMQ.Adapter");

        transportAdapter.CustomizeServiceControlTransport(
            customization: transport =>
            {
                transport.ConnectionString("host=localhost");
                var delayedDelivery = transport.DelayedDelivery();
                delayedDelivery.DisableTimeoutManager();

                transport.UseConventionalRoutingTopology();
            });

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapter.CustomizeEndpointTransport(
            customization: transport =>
            {
                transport.ConnectionString("host=localhost");
                var delayedDelivery = transport.DelayedDelivery();
                delayedDelivery.DisableTimeoutManager();

                transport.UseDirectRoutingTopology();
            });

        #endregion

#pragma warning restore 618

        #region AdapterQueueConfiguration

        transportAdapter.EndpointSideErrorQueue = "adapter_error";
        transportAdapter.EndpointSideAuditQueue = "adapter_audit";
        transportAdapter.EndpointSideControlQueue = "adapter_Particular.ServiceControl";

        transportAdapter.ServiceControlSideErrorQueue = "error";
        transportAdapter.ServiceControlSideAuditQueue = "audit";
        transportAdapter.ServiceControlSideControlQueue = "Particular.ServiceControl";

        #endregion

        var adapter = TransportAdapter.Create(transportAdapter);

        await adapter.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to shutdown adapter.");
        Console.ReadLine();

        await adapter.Stop()
            .ConfigureAwait(false);
    }
}