using Microsoft.Extensions.Hosting;

var endpointName = "Cinema.TicketSales";

if (args.Length > 0)
{
    endpointName = args[0];
}

Console.Title = endpointName;

var builder = Host.CreateApplicationBuilder(args);

// TODO: consider moving common endpoint configuration into a shared project
// for use by all endpoints in the system
var endpointConfiguration = new EndpointConfiguration(endpointName);

// Learning Transport: https://docs.net/transports/learning/
var routing = endpointConfiguration.UseTransport(new LearningTransport());

// Define routing for commands: https://docs.net/nservicebus/messaging/routing#command-routing
// routing.RouteToEndpoint(typeof(MessageType), "DestinationEndpointForType");
// routing.RouteToEndpoint(typeof(MessageType).Assembly, "DestinationForAllCommandsInAsembly");

// Learning Persistence: https://docs.net/persistence/learning/
endpointConfiguration.UsePersistence<LearningPersistence>();

// Message serialization
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

// Installers are useful in development. Consider disabling in production.
// https://docs.net/nservicebus/operations/installers
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

app.Run();

static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
{
    // TODO: decide if stopping the endpoint and exiting the process is the best response to a critical error
    // https://docs.net/nservicebus/hosting/critical-errors
    // and consider setting up service recovery
    // https://docs.net/nservicebus/hosting/windows-service#installation-restart-recovery
    try
    {
        await context.Stop(cancellationToken);
    }
    finally
    {
        FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
    }
}

static void FailFast(string message, Exception exception)
{
    try
    {
        // TODO: decide what kind of last resort logging is necessary
        // TODO: when using an external logging framework it is important to flush any pending entries prior to calling FailFast
        // https://docs.net/nservicebus/hosting/critical-errors#when-to-override-the-default-critical-error-action
    }
    finally
    {
        Environment.FailFast(message, exception);
    }

}
