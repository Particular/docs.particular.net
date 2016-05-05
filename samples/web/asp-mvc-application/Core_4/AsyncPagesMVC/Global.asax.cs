using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;
using NServiceBus.Installation.Environments;

public class MvcApplication : HttpApplication
{
    IBus bus;

    public static void RegisterRoutes(RouteCollection routes)
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

    public override void Dispose()
    {
        if (bus != null)
        {
            ((IDisposable)bus).Dispose();
        }
        base.Dispose();
    }

    protected void Application_Start()
    {
        #region ApplicationStart

        ContainerBuilder builder = new ContainerBuilder();

        // Register the MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        // Set the dependency resolver to be Autofac.
        IContainer container = builder.Build();

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Mvc.WebApplication");
        configure.AutofacBuilder(container);
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        configure.UnicastBus();
        bus = configure.CreateBus()
            .Start(() => configure.ForInstallationOn<Windows>().Install());

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);

        #endregion
    }
}