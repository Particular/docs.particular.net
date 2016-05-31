﻿// ReSharper disable SuggestVarOrType_Elsewhere
using System;
using NServiceBus;

class Usage
{
    void UseTransport(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");

        #endregion
    }

    void CodeOnly(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueConfigCodeOnly

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");
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
        transport.UseAccountNamesInsteadOfConnectionStrings();

        #endregion
    }

    void MultipleAccountNamesInsteadOfConnectionStrings1(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseMultipleAccountNamesInsteadOfConnectionStrings1

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("default_connection_string");
        transport.UseAccountNamesInsteadOfConnectionStrings(mapping =>
            {
                mapping.MapLocalAccount("default");
                mapping.MapAccount("another","another_connection_string");
            });

        #endregion
    }

    void MultipleAccountNamesInsteadOfConnectionStrings2(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseMultipleAccountNamesInsteadOfConnectionStrings2

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("another_connection_string");
        transport.UseAccountNamesInsteadOfConnectionStrings(mapping =>
            {
                mapping.MapLocalAccount("another");
                mapping.MapAccount("default", "default_connection_string");
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
            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");
        }
    }

    #endregion
}