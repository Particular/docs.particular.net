using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Routing;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.Azure.ServiceBus.Bridge";

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        #region bridge-general-configuration

        var bridgeConfiguration = new RouterConfiguration("Bridge");
        var azureInterface = bridgeConfiguration.AddInterface<AzureServiceBusTransport>("ASB", transport =>
        {
            //Prevents ASB from using TransactionScope
            transport.Transactions(TransportTransactionMode.ReceiveOnly);
            transport.ConnectionString(connectionString);
        });
        var msmqInterface = bridgeConfiguration.AddInterface<MsmqTransport>("MSMQ", transport =>
        {
            transport.Transactions(TransportTransactionMode.ReceiveOnly);
        });
        msmqInterface.EnableMessageDrivenPublishSubscribe(new InMemorySubscriptionStorage());

        //Configure the host of the MSMQ endpoint
        msmqInterface.EndpointInstances.AddOrReplaceInstances("publishers", new List<EndpointInstance>
        {
            new EndpointInstance("Samples.Azure.ServiceBus.MsmqEndpoint").AtMachine(Environment.MachineName)
        });

        bridgeConfiguration.AutoCreateQueues();

        var staticRouting = bridgeConfiguration.UseStaticRoutingProtocol();

        staticRouting.AddForwardRoute(
            incomingInterface: "MSMQ",
            outgoingInterface: "ASB");

        staticRouting.AddForwardRoute(
            incomingInterface: "ASB",
            outgoingInterface: "MSMQ");

        #endregion

        #region bridge-execution

        var bridge = Router.Create(bridgeConfiguration);

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await bridge.Stop().ConfigureAwait(false);


        #endregion
    }
}