using NServiceBus;

class SecureCredentials
{
    SecureCredentials(EndpointConfiguration endpointConfiguration)
    {
        #region enable_use_namespace_alias_instead_of_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseNamespaceAliasesInsteadOfConnectionStrings();

        #endregion
    }

    void MapDefaultLogicalNameToConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region map_default_logical_alias_to_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(
            connectionString: "Endpoint=sb://[NAMESPACE].servicebus.windows.net;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }
}