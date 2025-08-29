using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server;

Console.Title = "Server";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Server");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>()
    .BasePath(@"..\..\..\..\ClaimCheckShare\");

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
