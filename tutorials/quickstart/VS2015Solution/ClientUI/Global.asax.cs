using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ClientUI
{
    using System.Threading.Tasks;
    using ClientUI.Controllers;
    using Messages;
    using NServiceBus;

    public class MvcApplication : System.Web.HttpApplication
    {
        IEndpointInstance _endpointInstance;

        protected void Application_Start()
        {
            AsyncStart().GetAwaiter().GetResult();
        }
        
        async Task AsyncStart()
        {
            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            _endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new InjectEndpointInstanceIntoController(_endpointInstance));
        }

        protected void Application_End()
        {
            _endpointInstance?.Stop().GetAwaiter().GetResult();
        }
    }
}
