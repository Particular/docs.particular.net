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
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.ECommerce");
        busConfiguration.PurgeOnStartup(true);

        busConfiguration.ApplyCommonConfiguration();

        Bus = NServiceBus.Bus.Create(busConfiguration).Start();

        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

    protected void Application_End()
    {
        Bus?.Dispose();
    }
}