using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using NServiceBus;
using NServiceBus.Installation.Environments;

public class MvcApplication : HttpApplication
{
    public static IBus Bus;

    protected void Application_Start()
    {
        StartBus();
        AreaRegistration.RegisterAllAreas();
        RouteTable.Routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            }
            );
    }

    protected void Application_End()
    {
        ((IDisposable)Bus).Dispose();
    }

    void StartBus()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Callbacks.WebSender");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        Bus = configure.UnicastBus().CreateBus().Start(() => configure.ForInstallationOn<Windows>().Install());
    }
}