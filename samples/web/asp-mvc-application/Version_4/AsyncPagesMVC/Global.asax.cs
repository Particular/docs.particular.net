using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

namespace AsyncPagesMVC
{

    public class MvcApplication : System.Web.HttpApplication
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "SendLinks", id = UrlParameter.Optional } // Parameter defaults
                
            );
        }

        protected void Application_Start()
        {
            #region ApplicationStart
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof (MvcApplication).Assembly);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            Configure.Serialization.Json();
            var configure = Configure.With();
            configure.DefineEndpointName("Samples.Mvc.WebApplication");
            configure.AutofacBuilder(container);
            configure.InMemorySagaPersister();
            configure.UseInMemoryTimeoutPersister();
            configure.InMemorySubscriptionStorage();
            configure.UseTransport<Msmq>();
            configure.UnicastBus();
            configure.CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            #endregion
        }
    }
}