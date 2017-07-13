using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;

public class MvcApplication :
    HttpApplication
{
    public static IBus Bus;

    protected void Application_Start()
    {
        StartBus();
        AreaRegistration.RegisterAllAreas();
        RouteTable.Routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
    }

    protected void Application_End()
    {
        Bus.Dispose();
    }

    void StartBus()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Callbacks.WebSender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        Bus = NServiceBus.Bus.Create(busConfiguration)
            .Start();
    }
}