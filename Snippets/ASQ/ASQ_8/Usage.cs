using System;
using System.Threading.Tasks;
using NServiceBus;

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

    async Task SendToMulitpleAccountUsingAlias(IEndpointInstance endpointInstance)
    {
        #region storage_account_routing_send_options_alias

        await endpointInstance.Send(
            destination: "sales@accountName",
            message: new MyMessage());

        #endregion
    }

    void RegisterEndpoint(EndpointConfiguration configuration)
    {
        #region storage_account_routing_registered_endpoint

        var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
        var routing = transportConfig
                            .ConnectionString("connectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount("AnotherAccountName","anotherConnectionString");
        anotherAccount.RegisteredEndpoints.Add("Receiver");

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
        var routing = transportConfig
                            .ConnectionString("anotherConnectionString")
                            .AccountRouting();
        var anotherAccount = routing.AddAccount("PublisherAccountName", "connectionString");
        anotherAccount.RegisteredEndpoints.Add("Publisher");

        transportConfig.Routing().RegisterPublisher(typeof(MyEvent), "Publisher");

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

    public class MyMessage :
        ICommand
    { }

    public class MyEvent :
        IEvent
    { }

}
