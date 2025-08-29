using Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Client";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Client");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>()
    .BasePath(@"..\..\..\..\ClaimCheckShare\");

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();