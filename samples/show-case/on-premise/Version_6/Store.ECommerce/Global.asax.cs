using System.Threading.Tasks;
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
        AsyncStart().GetAwaiter().GetResult();
    }

    static async Task AsyncStart()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.ECommerce");
        busConfiguration.PurgeOnStartup(true);

        busConfiguration.ApplyCommonConfiguration();
        busConfiguration.SendFailedMessagesTo("error");

        Bus = await NServiceBus.Bus.Create(busConfiguration).StartAsync();

        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }
}
