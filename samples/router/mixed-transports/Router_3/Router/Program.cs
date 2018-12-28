using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.MixedTransports.Router";

        #region RouterConfig

        var routerConfig = new RouterConfiguration("Samples.Router.MixedTransports.Router");

        var msmqInterface = routerConfig.AddInterface<MsmqTransport>("MSMQ", t => { });
        msmqInterface.EnableMessageDrivenPublishSubscribe(new InMemorySubscriptionStorage());

        var rabbitMQInterface = routerConfig.AddInterface<RabbitMQTransport>("RabbitMQ", t =>
        {
            t.ConnectionString("host=localhost");
            t.UseConventionalRoutingTopology();
        });

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        staticRouting.AddForwardRoute("MSMQ", "RabbitMQ");
        routerConfig.AutoCreateQueues();

        #endregion

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop().ConfigureAwait(false);
    }
}