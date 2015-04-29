namespace Store.ECommerce
{
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NServiceBus;

    public class MvcApplication : HttpApplication
    {
        public static IBus Bus;

        protected void Application_Start()
        {
            var configuration = new BusConfiguration();
            configuration.PurgeOnStartup(true);
            configuration.RijndaelEncryptionService();

            configuration.UsePersistence<InMemoryPersistence>();

            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Store") && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Store") && t.Namespace.EndsWith("Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Store") && t.Namespace.EndsWith("RequestResponse"))
                .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));

            // In Production, make sure the necessary queues for this endpoint are installed before running the website
            if (Debugger.IsAttached)
            {
                // While calling this code will create the necessary queues required, this step should
                // ideally be done just one time as opposed to every execution of this endpoint.
                configuration.EnableInstallers();
            }

            Bus = NServiceBus.Bus.Create(configuration).Start();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

    }
}