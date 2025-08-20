using Microsoft.Extensions.Hosting;

Console.Title = "EndpointsMonitor";

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(x =>
{
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

    return endpointConfiguration;
});

var host = builder.Build();

await host.RunAsync();