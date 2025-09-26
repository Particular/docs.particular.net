using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Client";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Client");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>()
    .BasePath(@"..\..\..\..\ClaimCheckShare\");

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
await CommandSender.Start(messageSession);

await host.StopAsync();