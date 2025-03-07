using Headquarters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.Gateway;
using Shared;

Console.Title = "Headquarters";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.Headquarters");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region HeadquartersGatewayConfig
var gatewayConfig = endpointConfiguration.Gateway(new NonDurableDeduplicationConfiguration());
gatewayConfig.AddReceiveChannel("http://localhost:25899/Headquarters/");
gatewayConfig.AddSite("RemoteSite", "http://localhost:25899/RemoteSite/");
#endregion
Console.WriteLine("Press any key, application loading");
Console.ReadKey();
Console.WriteLine("Starting...");

Console.WriteLine("Press 'Enter' to send a message to RemoteSite which will reply.");
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();