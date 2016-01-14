using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;

public class MvcApplication : HttpApplication
{
    public static IBus Bus;

    public override void Dispose()
    {
        if (Bus != null)
        {
            Bus.Dispose();
        }
        base.Dispose();
    }

    protected void Application_Start()
    {
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Store.ECommerce");
        configuration.PurgeOnStartup(true);

        configuration.ApplyCommonConfiguration();

        Bus = NServiceBus.Bus.Create(configuration).Start();

        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

}