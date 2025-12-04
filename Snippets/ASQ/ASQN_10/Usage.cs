using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;

class Usage
{
    void UseTransport(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        var connectionString = "DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];";

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.UseQueueServiceClient(new QueueServiceClient(connectionString));

        //Blob client does not need to be specified if native delayed delivery is disabled
        //transport.DelayedDelivery().DisableDelayedDelivery();

        var account = CloudStorageAccount.Parse(connectionString);
        transport.UseCloudTableClient(new CloudTableClient(account.TableStorageUri, account.Credentials));
        transport.UseBlobServiceClient(new BlobServiceClient(connectionString));

        //Or alternatively
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");

        #endregion
    }

    void CodeOnly(EndpointConfiguration endpointConfiguration)
    {
        var queueServiceClient = new QueueServiceClient("connectionString", new QueueClientOptions());
        var blobServiceClient = new BlobServiceClient("connectionString", new BlobClientOptions());
        var cloudStorageAccount = CloudStorageAccount.Parse("connectionString");
        var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

        #region AzureStorageQueueConfigCodeOnly

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
        transport.BatchSize(20);
        transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
        transport.DegreeOfReceiveParallelism(16);
        transport.PeekInterval(TimeSpan.FromMilliseconds(100));
        transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));
        transport.UseQueueServiceClient(queueServiceClient);
        transport.UseBlobServiceClient(blobServiceClient);
        transport.UseCloudTableClient(cloudTableClient);

        #endregion
    }

    void RegisterMultipleStorageAccounts(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueuesAddingAdditionalAccounts

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("account_A_connection_string");
        transport.DefaultAccountAlias("account_A");
        var accountRouting = transport.AccountRouting();
        var remoteAccount = accountRouting.AddAccount("account_B", "account_B_connection_string");

        // Add an endpoint that receives commands
        remoteAccount.AddEndpoint("RemoteEndpoint");

        // Add endpoints that subscribe to events
        remoteAccount.AddEndpoint("RemoteSubscriberEndpoint");

        // Add endpoints that this endpoint publishes messages this endpoint subscribes to
        remoteAccount.AddEndpoint(endpointName: "RemotePublisher", publishedEvents: new[] { typeof(OrderAccepted) }, subscriptionTableName: "optionalSubscriptionTableName");

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

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.SubscriptionTableName("NewName");

        #endregion
    }

    void DisableCaching(EndpointConfiguration configuration)
    {
        #region storage_account_disable_subscription_caching

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.DisableCaching();

        #endregion
    }

    void ConfigureCaching(EndpointConfiguration configuration)
    {
        #region storage_account_configure_subscription_caching

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.CacheInvalidationPeriod(TimeSpan.FromSeconds(10));

        #endregion
    }

    public class MyMessage :
        ICommand
    { }

    public class OrderAccepted :
        IEvent
    { }

}
