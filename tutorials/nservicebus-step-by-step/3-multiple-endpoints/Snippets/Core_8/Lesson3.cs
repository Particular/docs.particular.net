using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Core_8
{
    class Snippets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("NServiceBus.Code", "NSB0002:Forward the 'CancellationToken' property of the context parameter to methods", Justification = "Parameters only both included for ease of documentation")]
        public async Task SendTypes(IEndpointInstance endpointInstance, IMessageHandlerContext context)
        {
            var command = new object();

            #region SendLocal
            // From endpoint startup code
            await endpointInstance.SendLocal(command);

            // From a message handler
            await context.SendLocal(command);
            #endregion

            #region Send
            // From endpoint startup code
            await endpointInstance.Send(command);

            // From a message handler
            await context.Send(command);
            #endregion

            #region SendDestination
            // Not recommended, most of the time
            await endpointInstance.Send("Destination", command);

            // On the IMessageHandlerContext too, but still not recommended
            await context.Send("Destination", command);

            #endregion

        }

        void ShowRouting(EndpointConfiguration endpointConfiguration)
        {
            #region RoutingSettings
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            // Returns a RoutingSettings<LearningTransport>
            var routing = transport.Routing();
            #endregion

            #region RouteToEndpoint
            // Specify the routing for a specific type
            routing.RouteToEndpoint(typeof(DoSomething), "SomeEndpoint");

            // Specify the routing for all messages in an assembly
            routing.RouteToEndpoint(typeof(DoSomething).Assembly, "SomeEndpoint");

            // Specify the routing for all messages in a given assembly and namespace
            routing.RouteToEndpoint(typeof(DoSomething).Assembly, "Specific.Namespace", "SomeEndpoint");

            #endregion
        }

        class DoSomething { }
        class PlaceOrder { }

        void ExerciseConfig()
        {
            #region EndpointDifferences
            Console.Title = "Sales";

            var endpointConfiguration = new EndpointConfiguration("Sales");
            #endregion

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            #region AddingRouting
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
            #endregion

        }
    }

}