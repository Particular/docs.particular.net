using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.Sites.RouterA";

        var routerConfig = new RouterConfiguration("SiteA");
        routerConfig.AddInterface<LearningTransport>("Local", t => { });
        routerConfig.AddInterface<MsmqTransport>("Tunnel", t => { }).EnableMessageDrivenPublishSubscribe(new NullSubscriptionStore());
        routerConfig.AutoCreateQueues();

        #region ConfigureRouterA

        var routing = routerConfig.UseStaticRoutingProtocol();
        routing.AddRoute(
            destinationFilter: (iface, destination) => destination.Site == "SiteB",
            destinationFilterDescription: "Forward to SiteB",
            gateway: "SiteB",
            iface: "Tunnel");

        #endregion


        var router = Router.Create(routerConfig);

        await router.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop()
            .ConfigureAwait(false);
    }
}