using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.Sites.RouterB";

        var routerConfig = new RouterConfiguration("SiteB");
        routerConfig.AddInterface<LearningTransport>("Local", t => { });
        routerConfig.AddInterface<MsmqTransport>("Tunnel", t => { }).EnableMessageDrivenPublishSubscribe(new NullSubscriptionStore());

        routerConfig.AutoCreateQueues();

        #region ConfigureRouterB

        var routing = routerConfig.UseStaticRoutingProtocol();
        routing.AddForwardRoute("Tunnel", "Local");

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