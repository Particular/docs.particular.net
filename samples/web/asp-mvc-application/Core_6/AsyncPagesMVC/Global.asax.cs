using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

public class MvcApplication :
    HttpApplication
{
    IEndpointInstance endpoint;

    static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new
            {
                controller = "Home",
                action = "SendLinks",
                id = UrlParameter.Optional
            } // Parameter defaults
            );
    }

    protected void Application_End()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    protected void Application_Start()
    {
        #region ApplicationStart

        var builder = new ContainerBuilder();

        // Register the MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);
        // Register the endpoint as a factory as the instance isn't created yet.
        builder.Register(ctx => endpoint).SingleInstance();
        
        var container = builder.Build();

        // Set the dependency resolver to be Autofac.
        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.WebApplication");
        endpointConfiguration.MakeInstanceUniquelyAddressable("1");
        endpointConfiguration.EnableCallbacks();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);

        #endregion
    }
}