using NServiceBus;
using NServiceBus.AzureServiceBus.Addressing;

class SecureCredentials
{
    SecureCredentials(EndpointConfiguration endpointConfiguration)
    {
        #region enable_use_namespace_name_instead_of_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseNamespaceNamesInsteadOfConnectionStrings();

        #endregion
    }

    void MapLogicalNameToConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region map_logical_name_to_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.UseStrategy<SingleNamespacePartitioning>();
        namespacePartitioning.AddNamespace("namespaceName", "Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    void MapDefaultLogiclaNameToConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region map_default_logical_name_to_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }
}