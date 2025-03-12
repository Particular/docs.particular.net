using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using SampleApp;


Console.Title = "DispatchNotification";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputService>();
#region endpoint-configuration
var endpointConfiguration = new EndpointConfiguration("Samples.DispatchNotification");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.NotifyDispatch(new SampleDispatchNotifier());
#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
