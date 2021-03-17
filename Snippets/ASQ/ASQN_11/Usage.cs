// ReSharper disable SuggestVarOrType_Elsewhere

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
class Usage
{
    void UseTransport(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        var transport = new AzureStorageQueueTransport("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CodeOnly(EndpointConfiguration endpointConfiguration)
    {
        var queueServiceClient = new QueueServiceClient("connectionString", new QueueClientOptions());
        var blobServiceClient = new BlobServiceClient("connectionString", new BlobClientOptions());
        var cloudStorageAccount = CloudStorageAccount.Parse("connectionString");
        var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

        #region AzureStorageQueueConfigCodeOnly

        var transport = new AzureStorageQueueTransport(queueServiceClient, blobServiceClient, cloudTableClient)
        {
            ReceiverBatchSize = 20,
            MaximumWaitTimeWhenIdle = TimeSpan.FromSeconds(1),
            DegreeOfReceiveParallelism = 16,
            PeekInterval = TimeSpan.FromMilliseconds(100),
            MessageInvisibleTime = TimeSpan.FromSeconds(30)
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    async Task SendToMulitpleAccountUsingConnectionSTring(IEndpointInstance endpointInstance)
    {
        #region storage_account_routing_send_options_full_connectionstring

        await endpointInstance.Send(
            destination: "sales@accountAlias",
            message: new MyMessage());

        #endregion
    }

    async Task SendToMulitpleAccountUsingAlias(IEndpointInstance endpointInstance)
    {
        #region storage_account_routing_send_options_alias

        await endpointInstance.Send(
            destination: "sales@accountAlias",
            message: new MyMessage());

        #endregion
    }

    void RegisterEndpoint(EndpointConfiguration configuration)
    {
#pragma warning disable CS0618
        #region storage_account_routing_registered_endpoint

        var transport = new AzureStorageQueueTransport("connectionString");

        var anotherAccount = transport.AccountRouting.AddAccount("AnotherAccountName", "anotherConnectionString");
        anotherAccount.AddEndpoint("Receiver");

        var routingConfig = configuration.UseTransport(transport);
        routingConfig.RouteToEndpoint(typeof(MyMessage), "Receiver");

        #endregion
#pragma warning restore CS0618
    }

    async Task SendToMulitpleAccountUsingRegisterdEndpoint(IEndpointInstance endpointInstance)
    {
        #region storage_account_routing_send_registered_endpoint

        await endpointInstance.Send(message: new MyMessage());

        #endregion
    }

    void RegisterPublisher(EndpointConfiguration configuration)
    {
#pragma warning disable CS0618
        #region storage_account_routing_registered_publisher

        var transport = new AzureStorageQueueTransport("connectionString");
        
        transport.AccountRouting.DefaultAccountAlias = "subscriber";

        var anotherAccount = transport.AccountRouting.AddAccount("publisher", "anotherConnectionString");
        anotherAccount.AddEndpoint("Publisher1", new[] { typeof(MyEvent)  }, "optionalSubscriptionTableName");

        configuration.UseTransport(transport);

        #endregion
#pragma warning restore CS0618
    }

    void RegisterSubscriber(EndpointConfiguration configuration)
    {
#pragma warning disable CS0618
        #region storage_account_routing_registered_subscriber

        var transport = new AzureStorageQueueTransport("anotherConnectionString");

        transport.AccountRouting.DefaultAccountAlias = "publisher";

        var anotherAccount = transport.AccountRouting.AddAccount("subscriber", "connectionString");
        anotherAccount.AddEndpoint("Subscriber1");

        configuration.UseTransport(transport);

        #endregion
#pragma warning restore CS0618
    }

    void SetSubscriptionTableName(EndpointConfiguration configuration)
    {
        #region storage_account_subscription_table_name

        var transport = new AzureStorageQueueTransport("connectionString");
        transport.Subscriptions.SubscriptionTableName = "NewName";

        #endregion
    }

    void DisableCaching(EndpointConfiguration configuration)
    {
        #region storage_account_disable_subscription_caching

        var transport = new AzureStorageQueueTransport("connectionString");
        transport.Subscriptions.DisableCaching = true;

        #endregion
    }

    void ConfigureCaching(EndpointConfiguration configuration)
    {
        #region storage_account_configure_subscription_caching

        var transport = new AzureStorageQueueTransport("connectionString");
        transport.Subscriptions.CacheInvalidationPeriod = TimeSpan.FromSeconds(10);

        #endregion
    }

    void MultipleAccountAliasesInsteadOfConnectionStrings1(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618
        #region AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

        var transport = new AzureStorageQueueTransport("account_A_connection_string");

        var accountRouting = transport.AccountRouting;
        accountRouting.DefaultAccountAlias = "account_A";
        accountRouting.AddAccount("account_B", "account_B_connection_string");

        endpointConfiguration.UseTransport(transport);

        #endregion
#pragma warning restore CS0618
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
            var transport = new AzureStorageQueueTransport("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
            endpointConfiguration.UseTransport(transport);
        }
    }

    #endregion
    // to avoid host ref
    internal interface IConfigureThisEndpoint
    {
    }

    public class MyMessage :
        ICommand
    { }

    public class MyEvent :
        IEvent
    { }

}
