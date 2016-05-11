// ReSharper disable SuggestVarOrType_Elsewhere
using System;
using NServiceBus;

class Usage
{
    void UseTransport(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

        #endregion
    }

    void CodeOnly(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueConfigCodeOnly

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("azure-storage-connection-string");
        transport.BatchSize(20);
        transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
        transport.DegreeOfReceiveParallelism(16);
        transport.PeekInterval(TimeSpan.FromMilliseconds(100));
        transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));

        #endregion
    }

    void AccountNamesInsteadOfConnectionStrings(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseAccountNamesInsteadOfConnectionStrings

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport
            .UseAccountNamesInsteadOfConnectionStrings();

        #endregion
    }

    void MultipleAccountNamesInsteadOfConnectionStrings(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseMultipleAccountNamesInsteadOfConnectionStrings

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport
            .UseAccountNamesInsteadOfConnectionStrings(mapping =>
            {
                mapping.MapLocalAccount("default"); // the account used by this endpoint
                mapping.MapAccount("another","azure_storage_account_connection_string"); // to enable sending messages to another account using the following address notation "queue_name@another"
            });

        #endregion
    }

    void UseSha1(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseSha1

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.UseSha1ForShortening();

        #endregion
    }

    #region AzureStorageQueueTransportWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        }
    }

    #endregion
}