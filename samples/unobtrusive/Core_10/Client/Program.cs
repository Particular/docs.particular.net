using Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Client";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Client");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
#pragma warning disable CS0618 // Type or member is obsolete
var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\DataBusShare\");
#pragma warning restore CS0618 // Type or member is obsolete

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();