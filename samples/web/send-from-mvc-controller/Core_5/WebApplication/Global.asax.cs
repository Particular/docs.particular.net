using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

public class MvcApplication :
    HttpApplication
{
    ISendOnlyBus bus;

    protected void Application_Start()
    {
        #region ApplicationStart

        var builder = new ContainerBuilder();

        // Register the MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        // Set the dependency resolver to be Autofac.
        var container = builder.Build();

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MvcInjection.WebApplication");
        // ExistingLifetimeScope() ensures that IBus is added to the container as well,
        // allowing resolve IBus from the components.
        busConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        bus = Bus.CreateSendOnly(busConfiguration);

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);

        #endregion
    }

    protected void Application_End()
    {
        bus?.Dispose();
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