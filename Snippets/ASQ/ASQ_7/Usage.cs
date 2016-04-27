// ReSharper disable SuggestVarOrType_Elsewhere
namespace ASQ_7
{
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
            transport.Addressing()
                .UseAccountNamesInsteadOfConnectionStrings();

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
}