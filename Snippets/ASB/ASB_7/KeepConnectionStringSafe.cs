namespace ASB_7
{
    using NServiceBus;
    using NServiceBus.AzureServiceBus.Addressing;

    public class KeepConnectionStringSafe
    {
        public KeepConnectionStringSafe(EndpointConfiguration endpointConfiguration)
        {
            #region enable_use_namespace_name_instead_of_connection_string

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseNamespaceNamesInsteadOfConnectionStrings();

            #endregion
        }

        public void MapLogicalNameToConnectionString(EndpointConfiguration endpointConfiguration)
        {
            #region map_logical_name_to_connection_string

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning()
                .UseStrategy<SingleNamespacePartitioning>()
                    .AddNamespace("namespaceName", "Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }

        public void MapDefaultLogiclaNameToConnectionString(EndpointConfiguration endpointConfiguration)
        {
            #region map_default_logical_name_to_connection_string

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString("Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }
    }
}