using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

namespace WebApplication
{
    public class MvcApplication : HttpApplication
    {
        ISendOnlyBus bus;

        protected void Application_Start()
        {
            #region ApplicationStart
            ContainerBuilder builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Set the dependency resolver to be Autofac.
            IContainer container = builder.Build();

            BusConfiguration configuration = new BusConfiguration();
            configuration.EndpointName("Samples.MvcInjection.WebApplication");
            configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
            configuration.UseSerialization<JsonSerializer>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.EnableInstallers();

            bus = Bus.CreateSendOnly(configuration);
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            #endregion
        }

        public override void Dispose()
        {
            if (bus != null)
            {
                bus.Dispose();
            }
            base.Dispose();
        }
    }
}
