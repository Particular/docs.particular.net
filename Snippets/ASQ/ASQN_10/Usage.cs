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

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();

        var routing = transportConfig
                            .ConnectionString("account_A_connection_string")
                            .AccountRouting();
        routing.DefaultAccountAlias = "account_A";
        
        var anotherAccount = routing.AddAccount("account_B","account_B_connection_string");
        anotherAccount.AddEndpoint("Receiver");

        transportConfig.Routing().RouteToEndpoint(typeof(MyMessage), "Receiver");

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

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.DefaultAccountAlias("subscriber");
        var routing = transportConfig
                            .ConnectionString("connectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount(alias: "publisher", connectionString: "anotherConnectionString");
        anotherAccount.AddEndpoint(endpointName: "Publisher1", publishedEvents: new[] { typeof(OrderAccepted) }, subscriptionTableName: "overrideForSubscriptionTableName");

        #endregion
    }

    void RegisterSubscriber(EndpointConfiguration configuration)
    {
        #region storage_account_routing_registered_subscriber

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.DefaultAccountAlias("publisher");
        var routing = transportConfig
                            .ConnectionString("anotherConnectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount(alias: "subscriber", connectionString: "connectionString");
        anotherAccount.AddEndpoint(endpointName: "Subscriber1");

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

    void MultipleAccountAliasesInsteadOfConnectionStrings1(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("account_A_connection_string");
        transport.DefaultAccountAlias("account_A");
        var accountRouting = transport.AccountRouting();
        accountRouting.AddAccount("account_B", "account_B_connection_string");

        #endregion
    }

    public class MyMessage :
        ICommand
    { }

    public class OrderAccepted :
        IEvent
    { }

}
