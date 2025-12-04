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

    void RegisterMultipleStorageAccounts(EndpointConfiguration endpointConfiguration)
    {
        #region AzureStorageQueuesAddingAdditionalAccounts

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("account_A_connection_string");
        transport.DefaultAccountAlias("account_A");
        var accountRouting = transport.AccountRouting();
        var remoteAccount = accountRouting.AddAccount("account_B", "account_B_connection_string");

        // Add an endpoint that receives commands
        remoteAccount.RegisteredEndpoints.Add("RemoteEndpoint");

        // Add endpoints that subscribe to events
        remoteAccount.RegisteredEndpoints.Add("RemoteSubscriberEndpoint");

        // Add endpoints that this endpoint publishes messages this endpoint subscribes to
        // This is not supported in this version
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

    public class MyMessage :
        ICommand
    { }

    public class MyEvent :
        IEvent
    { }

}
