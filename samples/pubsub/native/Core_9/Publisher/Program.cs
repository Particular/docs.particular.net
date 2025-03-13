using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Publisher;

Console.Title = "Publisher";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Publisher");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
Console.WriteLine("Press any key, application loading");
Console.ReadKey();
Console.WriteLine("Starting...");
builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();