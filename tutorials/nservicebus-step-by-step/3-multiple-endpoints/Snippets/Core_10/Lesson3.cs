using Microsoft.Extensions.Hosting;

namespace Core;

class Snippets
{
#pragma warning disable NSB0002 // Forward the 'CancellationToken' property of the context parameter to methods
    public async Task SendTypes(IMessageSession messageSession, IMessageHandlerContext context)
    {
        var command = new object();

        #region SendLocal
        // From endpoint startup code

        await messageSession.SendLocal(command);


        // From a message handler
        await context.SendLocal(command);
        #endregion

        #region Send
        // From endpoint startup code
        await messageSession.Send(command);

        // From a message handler
        await context.Send(command);
        #endregion

        #region SendDestination
        // Not recommended, most of the time
        await messageSession.Send("Destination", command);

        // On the IMessageHandlerContext too, but still not recommended
        await context.Send("Destination", command);

        #endregion

    }
#pragma warning restore NSB0002 // Forward the 'CancellationToken' property of the context parameter to methods

    public void ShowRouting(EndpointConfiguration endpointConfiguration)
    {
        #region RoutingSettings
        // Returns a RoutingSettings<LearningTransport>
        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        #endregion

        #region RouteToEndpoint
        // Specify the routing for a specific type
        routing.RouteToEndpoint(typeof(DoSomething), "SomeEndpoint");

        // Specify the routing for all messages in an assembly
        routing.RouteToEndpoint(typeof(DoSomething).Assembly, "SomeEndpoint");

        // Specify the routing for all messages in a given assembly and namespace
        routing.RouteToEndpoint(typeof(DoSomething).Assembly, "Namespace.A", "SomeEndpoint");

        #endregion
    }

    class DoSomething { }

    class PlaceOrder { }

    public void ExerciseConfig()
    {
        #region EndpointDifferences
        Console.Title = "Sales";

        var endpointConfiguration = new EndpointConfiguration("Sales");
        #endregion

        var routing = endpointConfiguration.UseTransport(new LearningTransport());

        #region AddingRouting
        routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
        #endregion

    }

    public static async Task Program(string[] args)
    {
        #region SalesConsoleApp
        Console.Title = "Sales";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Sales");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        builder.UseNServiceBus(endpointConfiguration);

        var app = builder.Build();

        await app.RunAsync();
        #endregion
    }
}
