namespace Store.ECommerce
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NServiceBus;
    using Store.Shared;

    public class MvcApplication : HttpApplication
    {
        public static IBus Bus;

        protected void Application_Start()
        {
            var configuration = new BusConfiguration();
            configuration.EndpointName("Store.ECommerce");
            configuration.PurgeOnStartup(true);

            configuration.ApplyCommonConfiguration();


            Bus = NServiceBus.Bus.Create(configuration).Start();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

    }
}