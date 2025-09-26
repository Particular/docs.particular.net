using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Server");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
#pragma warning disable CS0618 // Type or member is obsolete
endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>()
    .BasePath(@"..\..\..\..\DataBusShare\");
#pragma warning restore CS0618 // Type or member is obsolete

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
await CommandSender.Start(messageSession);

await host.StopAsync();
