// ReSharper disable SuggestVarOrType_Elsewhere

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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

    async Task SendToMulitpleAccountUsingConnectionSTring(IEndpointInstance endpointInstance)
    {
        #region storage_account_routing_send_options_full_connectionstring

        await endpointInstance.Send(
            destination: "sales@DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];",
            message: new MyMessage());

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

    public class MyMessage :
        ICommand
    { }

    public class MyEvent :
        IEvent
    { }

}
