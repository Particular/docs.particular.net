using System.Configuration;
using NServiceBus;

public class Upgrade
{
    void PurgeOnStartup(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueuePurgeOnStartup

        endpointConfiguration.PurgeOnStartup(true);

        #endregion
    }
    void UseTransport(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7AzureStorageQueueTransportWithAzure

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        // Configure the transport
        transport.ConnectionString("The Connection String");

        #endregion
    }
    void StillUseConfig(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7ConnectionStringFromConfig

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var connection = ConfigurationManager.AppSettings["AzureStorageQueueConnection"];
        transport.ConnectionString(connection);

        #endregion
    }
}