using System.Configuration;
using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }
}