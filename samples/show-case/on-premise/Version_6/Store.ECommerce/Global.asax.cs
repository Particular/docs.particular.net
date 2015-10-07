namespace Store.ECommerce
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NServiceBus;
    using Store.Shared;

    public class MvcApplication : HttpApplication
    {
        public static IBus Bus;

        public override void Dispose()
        {
            if (Bus != null)
            {
                Bus.Dispose();
            }
            base.Dispose();
        }

        protected void Application_Start()
        {
            AsyncStart().GetAwaiter().GetResult();
        }

        private static async Task AsyncStart()
        {
            var configuration = new BusConfiguration();
            configuration.EndpointName("Store.ECommerce");
            configuration.PurgeOnStartup(true);

            configuration.ApplyCommonConfiguration();

            Bus = await NServiceBus.Bus.Create(configuration).StartAsync();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}