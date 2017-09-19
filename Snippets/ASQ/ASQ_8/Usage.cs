// ReSharper disable SuggestVarOrType_Elsewhere

using System;
using System.Diagnostics.CodeAnalysis;
using NServiceBus;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
class Usage
{
    void UseTransport(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");

        #endregion
    }

    void CodeOnly(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueConfigCodeOnly

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
        transport.BatchSize(20);
        transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
        transport.DegreeOfReceiveParallelism(16);
        transport.PeekInterval(TimeSpan.FromMilliseconds(100));
        transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));

        #endregion
    }

    void AccountAliasesInsteadOfConnectionStrings(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseAccountAliasesInsteadOfConnectionStrings

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.UseAccountAliasesInsteadOfConnectionStrings();

        #endregion
    }

    void MultipleAccountAliasesInsteadOfConnectionStrings1(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("account_A_connection_string");
        transport.UseAccountAliasesInsteadOfConnectionStrings();
        transport.DefaultAccountAlias("account_A");
        var accountRouting = transport.AccountRouting();
        accountRouting.AddAccount("account_B", "account_B_connection_string");

        #endregion
    }

    void MultipleAccountAliasesInsteadOfConnectionStrings2(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings2

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("account_B_connection_string");
        transport.UseAccountAliasesInsteadOfConnectionStrings();
        transport.DefaultAccountAlias("account_B");
        var accountRouting = transport.AccountRouting();
        accountRouting.AddAccount("account_A", "account_A_connection_string");

        #endregion
    }

    void UseSha1(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueSanitization

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.SanitizeQueueNamesWith(queueName => queueName.Replace(".", "-"));

        #endregion
    }

    void SetSerialization(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueSerialization

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        #endregion
    }

    // Reference NServiceBus.Host instead of NServiceBus.Hosting.Azure as the usage is the same
    // This prevents Azure storage library conflicts as the libraries are being updated

    #region AzureStorageQueueTransportWithAzureHost

    public class EndpointConfig :
        IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
        }
    }

    #endregion
    // to avoid host ref
    internal interface IConfigureThisEndpoint
    {
    }
}
