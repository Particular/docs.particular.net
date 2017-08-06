﻿using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Core_6
{
    class Snippets
    {
        public async Task SendTypes(IEndpointInstance endpointInstance, IMessageHandlerContext context)
        {
            var command = new object();

            #region SendLocal
            // From endpoint startup code
            await endpointInstance.SendLocal(command)
                .ConfigureAwait(false);

            // From a message handler
            await context.SendLocal(command)
                .ConfigureAwait(false);
            #endregion

            #region Send
            // From endpoint startup code
            await endpointInstance.Send(command)
                .ConfigureAwait(false);

            // From a message handler
            await context.Send(command)
                .ConfigureAwait(false);
            #endregion

            #region SendDestination
            // Not recommended, most of the time
            await endpointInstance.Send("Destination", command)
                .ConfigureAwait(false);

            // On the IMessageHandlerContext too, but still not recommended
            await context.Send("Destination", command)
                .ConfigureAwait(false);

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