using AzureFunctions.ServiceBus;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

#region service-bus-program-builder

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddNServiceBusFunctions();

builder.AddSendOnlyNServiceBusEndpoint("client", configuration =>
{
    var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default);

    var routing = configuration.UseTransport(transport);
    routing.RouteToEndpoint(typeof(TriggerMessage), "Orders");
    configuration.UseSerialization<SystemJsonSerializer>();
});

var host = builder.Build();
await host.RunAsync();

#endregion