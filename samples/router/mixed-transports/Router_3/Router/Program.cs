using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.TrafficMirroring.Router";

        #region RouterConfig

        var routerConfig = new RouterConfiguration("Samples.Router.TrafficMirroring.Router");

        var prodInterface = routerConfig.AddInterface<AzureServiceBusTransport>("Prod", t =>
        {
            t.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
            t.Transactions(TransportTransactionMode.ReceiveOnly);
        });

        var testInterface = routerConfig.AddInterface<AzureServiceBusTransport>("Test", t =>
        {
            t.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.2"));
            t.Transactions(TransportTransactionMode.ReceiveOnly);
        });

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        staticRouting.AddForwardRoute("Prod", "Test");
        routerConfig.AutoCreateQueues();

        #endregion

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop().ConfigureAwait(false);
    }
}