using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Serialization;
using NServiceBus.Settings;
using ServiceControl.TransportAdapter;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.ASQAdapter.Adapter";

        #region AdapterTransport

        var transportAdapterConfig =
            new TransportAdapterConfig<AzureStorageQueueTransport, AzureStorageQueueTransport>("ServiceControl-ASQ-Adapter");

        #endregion

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureStorageQueue.ConnectionString.Endpoints");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception("Could not read 'AzureStorageQueue.ConnectionString.Endpoints' environment variable. Check sample prerequisites.");
                }

                transport.ConnectionString(connectionString);
                transport.DefaultAccountAlias("storage_account");

                // Required to address https://github.com/Particular/NServiceBus.AzureStorageQueues/issues/308
                transport.AccountRouting().AddAccount("storage_account", connectionString);

                #region serializer-workaround

                var settings = transport.GetSettings();

                // Register serializer used to serialize MessageWrapper (custom MessageWrapper serializer or endpoint's serializer different than JSON)
                var serializer = Tuple.Create(new NewtonsoftSerializer() as SerializationDefinition, new SettingsHolder());
                settings.Set("MainSerializer", serializer);

                #endregion
            });

        #endregion

        #region SCSideConfig

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureStorageQueue.ConnectionString.SC");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception("Could not read 'AzureStorageQueue.ConnectionString.SC' environment variable. Check sample prerequisites.");
                }

                transport.ConnectionString(connectionString);

                #region serializer-workaround

                var settings = transport.GetSettings();

                // Register serializer used to serialize MessageWrapper (custom MessageWrapper serializer or endpoint's serializer different than JSON)
                var serializer = Tuple.Create(new NewtonsoftSerializer() as SerializationDefinition, new SettingsHolder());
                settings.Set("MainSerializer", serializer);

                #endregion

            });

        #endregion

        #region UseStorageAccountHeader

        transportAdapterConfig.RedirectRetriedMessages((failedQ, headers) =>
        {
            if (headers.TryGetValue(AdapterSpecificHeaders.OriginalStorageAccountAlias, out var storageAccountAlias))
            {
                return $"{failedQ}@{storageAccountAlias}";
            }
            return failedQ;
        });

        #endregion

        #region ControlQueueOverride

        transportAdapterConfig.ServiceControlSideControlQueue = "Particular-ServiceControl-ASQ";

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
