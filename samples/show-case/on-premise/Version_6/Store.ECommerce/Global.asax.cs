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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Store.ECommerce");
        endpointConfiguration.PurgeOnStartup(true);

        endpointConfiguration.ApplyCommonConfiguration();
        endpointConfiguration.SendFailedMessagesTo("error");

        Endpoint = await NServiceBus.Endpoint.Start(endpointConfiguration);

        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }
}
