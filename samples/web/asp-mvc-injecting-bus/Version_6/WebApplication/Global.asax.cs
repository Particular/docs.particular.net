using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

public class MvcApplication : HttpApplication
{
    IEndpointInstance endpoint;

    protected void Application_Start()
    {
        #region ApplicationStart

        ContainerBuilder builder = new ContainerBuilder();

        // Register your MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        // Set the dependency resolver to be Autofac.
        IContainer container = builder.Build();

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MvcInjection.WebApplication");
        // ExistingLifetimeScope() ensures that IBus is added to the container as well,
        // allowing you to resolve IBus from your own components.
        busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");

        endpoint = Endpoint.Start(busConfiguration).GetAwaiter().GetResult();
        ContainerBuilder updater = new ContainerBuilder();
        updater.RegisterInstance(endpoint);
        updater.Update(container);

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);

        #endregion
    }

    public override void Dispose()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
        base.Dispose();
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