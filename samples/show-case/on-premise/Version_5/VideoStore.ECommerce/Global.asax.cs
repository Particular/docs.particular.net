namespace VideoStore.ECommerce
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

            // For production use, please select a durable persistence.
            // To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
            // To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();
            if (Debugger.IsAttached)
            {
                configuration.UsePersistence<InMemoryPersistence>();
            }

            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("RequestResponse"))
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