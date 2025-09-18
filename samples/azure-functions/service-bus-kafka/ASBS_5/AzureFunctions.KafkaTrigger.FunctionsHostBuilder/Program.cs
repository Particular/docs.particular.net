using AzureFunctions.Messages.NServiceBusMessages;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#region SetupNServiceBusSendOnly

var builder = FunctionsApplication.CreateBuilder(args);

var cfg = new EndpointConfiguration("SendOnly");
cfg.SendOnly();
cfg.UseSerialization<SystemJsonSerializer>();

var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
var routing = cfg.UseTransport(transport);

routing.RouteToEndpoint(typeof(FollowUp), "Samples.KafkaTrigger.ConsoleEndpoint");

var endpoint = await Endpoint.Start(cfg);

// Inject the endpoint in the DI container
builder.Services.AddSingleton<IMessageSession>(endpoint);

var host = builder.Build();

#endregion

await host.RunAsync();