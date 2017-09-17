using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.ASBAdapter.Adapter";
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<AzureServiceBusTransport, AzureServiceBusTransport>("ServiceControl.ASB.Adapter");

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {
                var salesConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.1");
                if (string.IsNullOrWhiteSpace(salesConnectionString))
                {
                    throw new Exception("Could not read 'AzureServiceBus.ConnectionString.1' environment variable. Check sample prerequisites.");
                }
                var shippingConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.2");
                if (string.IsNullOrWhiteSpace(shippingConnectionString))
                {
                    throw new Exception("Could not read 'AzureServiceBus.ConnectionString.2' environment variable. Check sample prerequisites.");
                }

                transport.UseNamespaceAliasesInsteadOfConnectionStrings();
                var namespacePartitioning = transport.NamespacePartitioning();
                namespacePartitioning.AddNamespace("sales", salesConnectionString);
                namespacePartitioning.AddNamespace("shipping", shippingConnectionString);
                namespacePartitioning.UseStrategy<RoundRobinNamespacePartitioning>();
                transport.UseForwardingTopology();
                var composition = transport.Composition();
                composition.UseStrategy<HierarchyComposition>()
                    .PathGenerator(path => "scadapter/");
            });

        #endregion

#pragma warning restore 618

        #region SCSideConfig

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.SC");
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    transport.ConnectionString(connectionString);
                    transport.UseEndpointOrientedTopology();
                    return;
                }
                throw new Exception("Could not read 'AzureServiceBus.ConnectionString.SC' environment variable. Check sample prerequisites.");
            });

        #endregion

        #region UseNamespaceHeader

        transportAdapterConfig.RedirectRetriedMessages((failedQ, headers) =>
        {
            if (headers.TryGetValue(AdapterSpecificHeaders.OriginalNamespace, out var namespaceAlias))
            {
                return $"{failedQ}@{namespaceAlias}";
            }
            return failedQ;
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