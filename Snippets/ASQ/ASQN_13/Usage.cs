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

    void RegisterMultipleStorageAccounts(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueuesAddingAdditionalAccounts

        var transport = new AzureStorageQueueTransport("account_A_connection_string");
        transport.AccountRouting.DefaultAccountAlias = "account_A";

        var remoteAccount = transport.AccountRouting.AddAccount(
            "account_B",
            new QueueServiceClient("account_B_connection_string"),
            new TableServiceClient("account_B_connection_string"));

        // Add an endpoint that receives commands
        remoteAccount.AddEndpoint("RemoteEndpoint");

        // Add endpoints that subscribe to events
        remoteAccount.AddEndpoint("RemoteSubscriberEndpoint");

        // Add endpoints that this endpoint publishes messages this endpoint subscribes to
        remoteAccount.AddEndpoint("RemotePublisher", new[] { typeof(MyEvent)  }, "optionalSubscriptionTableName");

        #endregion
    }

    async Task SendOptionsReplyWithAccountAlias(IMessageHandlerContext context)
    {
        #region AzureStorageSendOptionsReply

        var sendOptions = new SendOptions();
        sendOptions.RouteReplyTo("sales@accountAlias");

        await context.Send(
            message: new MyMessage(),
            options: sendOptions);

        #endregion
    }

    async Task SendOptionsOverrideWithAccountAlias(IMessageHandlerContext context)
    {
        #region AzureStorageSendOptionsOverride

        var sendOptions = new SendOptions();
        sendOptions.SetDestination("sales@accountAlias");

        await context.Send(
            message: new MyMessage(),
            options: sendOptions);

        //Or with a helper extension method:

        await context.Send(
            destination: "sales@accountAlias",
            message: new MyMessage());

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

    public class MyMessage :
        ICommand
    { }

    public class MyEvent :
        IEvent
    { }

}
