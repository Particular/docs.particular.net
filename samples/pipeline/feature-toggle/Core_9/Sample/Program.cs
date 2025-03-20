using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sample;

Console.Title = "PipelineFeatureToggle";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputService>();
var endpointConfiguration = new EndpointConfiguration("Samples.PipelineFeatureToggle");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region enable-feature

var toggles = endpointConfiguration.EnableFeatureToggles();
toggles.AddToggle(ctx => ctx.MessageHandler.HandlerType == typeof(Handler2));


#endregion



Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
