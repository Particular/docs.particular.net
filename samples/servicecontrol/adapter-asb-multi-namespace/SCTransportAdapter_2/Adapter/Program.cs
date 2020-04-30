using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Settings;
using ServiceControl.TransportAdapter;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.ASBAdapter.Adapter";
        #region AdapterTransport

#pragma warning disable 618
        var transportAdapterConfig = new TransportAdapterConfig<AzureServiceBusTransport, AzureServiceBusTransport>("ServiceControl.ASB.Adapter");
#pragma warning restore 618

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

                WorkaroundForServializerRequiredByASB(transport);
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
                    throw new Exception("Could not read 'AzureServiceBus.ConnectionString.SC' environment variable. Check sample prerequisites.");
                }
                transport.ConnectionString(connectionString);
                transport.UseEndpointOrientedTopology();

                WorkaroundForServializerRequiredByASB(transport);
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

#pragma warning disable 618
    static void WorkaroundForServializerRequiredByASB(TransportExtensions<AzureServiceBusTransport> transport)
#pragma warning restore 618
    {
        var settings = transport.GetSettings();
        var serializer = Tuple.Create(new FakeSerializer() as SerializationDefinition, new SettingsHolder());
        settings.Set("MainSerializer", serializer);
    }

    class FakeSerializer : SerializationDefinition
    {
        public override Func<IMessageMapper, IMessageSerializer> Configure(ReadOnlySettings settings)
        {
            throw new NotImplementedException();
        }
    }
}