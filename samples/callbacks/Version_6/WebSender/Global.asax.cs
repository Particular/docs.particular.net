using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using NServiceBus;

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
        Bus.Dispose();
    }

    public static void StartBus()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Callbacks.WebSender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");

        Bus = NServiceBus.Bus.Create(busConfiguration)
            .Start();
    }
}