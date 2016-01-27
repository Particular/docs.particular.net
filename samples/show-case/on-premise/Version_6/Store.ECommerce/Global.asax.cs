using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;

public class MvcApplication : HttpApplication
{
    public static IEndpointInstance Endpoint;

    public override void Dispose()
    {
        Endpoint?.Stop().GetAwaiter().GetResult();
        base.Dispose();
    }

    protected void Application_Start()
    {
        AsyncStart().GetAwaiter().GetResult();
    }

    static async Task AsyncStart()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.ECommerce");
        busConfiguration.PurgeOnStartup(true);

        busConfiguration.ApplyCommonConfiguration();
        busConfiguration.SendFailedMessagesTo("error");

        Endpoint = await NServiceBus.Endpoint.Start(busConfiguration);

        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }
}
