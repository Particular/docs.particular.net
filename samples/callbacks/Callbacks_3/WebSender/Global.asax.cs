using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;

public class MvcApplication :
    HttpApplication
{
    public static IEndpointInstance EndpointInstance;

    protected void Application_Start()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        StartBus().GetAwaiter().GetResult();
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
        EndpointInstance?.Stop().GetAwaiter().GetResult();
    }

    async Task StartBus()
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.WebSender");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.MakeInstanceUniquelyAddressable("1");
        endpointConfiguration.EnableCallbacks();

        EndpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
    }

}