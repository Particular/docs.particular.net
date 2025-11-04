using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Data.Tables;
using NServiceBus;

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
        #region AzureStorageQueueConfigCodeOnly

        var queueServiceClient = new QueueServiceClient("connectionString", new QueueClientOptions());
        var blobServiceClient = new BlobServiceClient("connectionString", new BlobClientOptions());
        var tableServiceClient = new TableServiceClient("connectionString", new TableClientOptions());

        var transport = new AzureStorageQueueTransport(queueServiceClient, blobServiceClient, tableServiceClient)
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
        #region storage_account_routing_registered_endpoint

        var transport = new AzureStorageQueueTransport("account_A_connection_string");
        var routing = transport.AccountRouting();
        routing.DefaultAccountAlias = "account_A";
        
        var anotherAccount = transport.AccountRouting.AddAccount(
            "account_B",
            new QueueServiceClient("account_B_connection_string"),
            new TableServiceClient("account_B_connection_string"));
        anotherAccount.AddEndpoint("Receiver");

        var routingConfig = configuration.UseTransport(transport);
        routingConfig.RouteToEndpoint(typeof(MyMessage), "Receiver");

        #endregion
    }

    async Task SendToMulitpleAccountUsingRegisterdEndpoint(IEndpointInstance endpointInstance)
    {
        #region storage_account_routing_send_registered_endpoint

        await endpointInstance.Send(message: new MyMessage());

        #endregion
    }

    void RegisterPublisher(EndpointConfiguration configuration)
    {
        #region storage_account_routing_registered_publisher

        var transport = new AzureStorageQueueTransport("connectionString");

        transport.AccountRouting.DefaultAccountAlias = "subscriber";

        var anotherAccount = transport.AccountRouting.AddAccount(
            "publisher",
            new QueueServiceClient("anotherConnectionString"),
            new TableServiceClient("anotherConnectionString"));
        anotherAccount.AddEndpoint("Publisher1", new[] { typeof(MyEvent)  }, "optionalSubscriptionTableName");

        configuration.UseTransport(transport);

        #endregion
    }

    void RegisterSubscriber(EndpointConfiguration configuration)
    {
        #region storage_account_routing_registered_subscriber

        var transport = new AzureStorageQueueTransport("anotherConnectionString");

        transport.AccountRouting.DefaultAccountAlias = "publisher";

        var anotherAccount = transport.AccountRouting.AddAccount(
            "subscriber",
            new QueueServiceClient("connectionString"),
            new TableServiceClient("connectionString"));
        anotherAccount.AddEndpoint("Subscriber1");

        configuration.UseTransport(transport);

        #endregion
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
        #region AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

        var transport = new AzureStorageQueueTransport("account_A_connection_string");

        var accountRouting = transport.AccountRouting;
        accountRouting.DefaultAccountAlias = "account_A";
        accountRouting.AddAccount(
            "account_B",
            new QueueServiceClient("account_B_connection_string"),
            new TableServiceClient("account_B_connection_string"));

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public class MyMessage :
        ICommand
    { }

    public class MyEvent :
        IEvent
    { }

}
