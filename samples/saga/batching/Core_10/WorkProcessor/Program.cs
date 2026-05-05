using Microsoft.Extensions.Hosting;
using NServiceBus;

var config = new EndpointConfiguration(EndpointNames.WorkProcessor);

config.UsePersistence<LearningPersistence>();
config.UseTransport<LearningTransport>();
config.UseSerialization<SystemJsonSerializer>();
config.AuditProcessedMessagesTo("audit");
config.SendFailedMessagesTo("error");
config.LimitMessageProcessingConcurrencyTo(64);

var transport = config.UseTransport<LearningTransport>();
var routing = transport.Routing();
RoutingHelper.ApplyDefaultRouting(routing);

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(config);
await builder.Build().RunAsync();