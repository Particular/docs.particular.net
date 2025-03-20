using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server;

Console.Title = "Server";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Server");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
#pragma warning disable CS0618 // Type or member is obsolete
endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>()
    .BasePath(@"..\..\..\..\DataBusShare\");
#pragma warning restore CS0618 // Type or member is obsolete

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
