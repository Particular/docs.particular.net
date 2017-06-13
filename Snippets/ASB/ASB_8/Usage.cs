using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }

    void SettingConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region setting_asb_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }
}