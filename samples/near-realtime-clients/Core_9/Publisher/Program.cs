using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Publisher;

Console.Title = "Publisher";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputService>();
var endpointConfiguration = new EndpointConfiguration("Samples.NearRealTimeClients.Publisher");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.SendFailedMessagesTo("error");

#region MessageConventionsForNonNSB

var conventions = endpointConfiguration.Conventions();
conventions.DefiningEventsAs(type => type == typeof(StockTick));

#endregion

endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
