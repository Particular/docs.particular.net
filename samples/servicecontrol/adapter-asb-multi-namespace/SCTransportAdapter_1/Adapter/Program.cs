using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ServiceControl.ASBAdapter.Adapter";
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<AzureServiceBusTransport, AzureServiceBusTransport>("ServiceControl.ASB.Adapter");

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(transport =>
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
            }
            transport.UseNamespaceAliasesInsteadOfConnectionStrings();
            transport.ConnectionString(connectionString);
            transport.UseForwardingTopology();
        });

        #endregion

#pragma warning restore 618

        #region SCSideConfig

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.SC");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception("Could not read the 'AzureServiceBus.ConnectionString.SC' environment variable. Check the sample prerequisites.");
                }
                transport.ConnectionString(connectionString);
                transport.UseEndpointOrientedTopology();
            });

        #endregion

        #region ControlQueueOverride

        transportAdapterConfig.ServiceControlSideControlQueue = "Particular.ServiceControl.ASB";

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