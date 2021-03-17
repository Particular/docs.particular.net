using NServiceBus;

class NativePubSub
{
    void RegisterEndpoint(EndpointConfiguration configuration)
    {
        #region 8to9-storage_account_routing_registered_endpoint

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        var routing = transportConfig
                            .ConnectionString("connectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount("AnotherAccountName", "anotherConnectionString");
        anotherAccount.RegisteredEndpoints.Add("Receiver");

        transportConfig.Routing().RouteToEndpoint(typeof(MyMessage), "Receiver");

        #endregion
    }

    void RegisterPublisher(EndpointConfiguration configuration)
    {
        #region 8to9-storage_account_routing_registered_publisher

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.DefaultAccountAlias("subscriber");
        var routing = transportConfig
                            .ConnectionString("connectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount("PublisherAccountName", "anotherConnectionString");
        anotherAccount.RegisteredEndpoints.Add("Publisher");

        transportConfig.Routing().RegisterPublisher(typeof(MyEvent), "Publisher");

        #endregion
    }

    void RegisterSubscriber(EndpointConfiguration configuration)
    {
        #region 8to9-storage_account_routing_registered_subscriber

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        transportConfig.DefaultAccountAlias("publisher");
        var routing = transportConfig
                            .ConnectionString("anotherConnectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount("subscriber", "connectionString");
        anotherAccount.RegisteredEndpoints.Add("Subscriber1");
        #endregion
    }

    public class MyMessage :
       ICommand
    { }

    public class MyEvent :
        IEvent
    { }
}
