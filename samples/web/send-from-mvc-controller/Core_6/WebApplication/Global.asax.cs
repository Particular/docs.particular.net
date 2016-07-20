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


    protected void Application_Start()
    {
        #region ApplicationStart

        var builder = new ContainerBuilder();

        // Register MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        // Set the dependency resolver to be Autofac.
        var container = builder.Build();

        var endpointConfiguration = new EndpointConfiguration("Samples.MvcInjection.WebApplication");
        // instruct NServiceBus to use a custom AutoFac configuration
        endpointConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        var updater = new ContainerBuilder();
        updater.RegisterInstance(endpoint);
        updater.Update(container);

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);

        #endregion
    }

    protected void Application_End()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    void RegisterRoutes(RouteCollection routes)
    {
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "Default",
                action = "Index",
                id = UrlParameter.Optional
            }
        );
    }
}