using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClientUI
{
    using System;
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;

    public class MvcApplication : HttpApplication
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

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

            var metrics = endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

            _endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new InjectEndpointInstanceIntoController(_endpointInstance));
        }

        protected void Application_End()
        {
            _endpointInstance?.Stop().GetAwaiter().GetResult();
        }
    }
}
