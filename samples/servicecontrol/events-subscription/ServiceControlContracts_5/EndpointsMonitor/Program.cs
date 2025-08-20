using Microsoft.Extensions.Hosting;

Console.Title = "EndpointsMonitor";

var endpointConfiguration = new EndpointConfiguration("EndpointsMonitor");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

#region ServiceControlEventsMonitorCustomErrorQueue
endpointConfiguration.SendFailedMessagesTo("error-monitoring");
#endregion

var transport = endpointConfiguration.UseTransport<LearningTransport>();

var conventions = endpointConfiguration.Conventions();
conventions.DefiningEventsAs(
    type =>
    {
        return typeof(IEvent).IsAssignableFrom(type) ||
               // include ServiceControl events
               type.Namespace != null &&
               type.Namespace.StartsWith("ServiceControl.Contracts");
    });

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();