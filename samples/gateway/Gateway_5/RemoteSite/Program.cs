using Microsoft.Extensions.Hosting;
using NServiceBus.Gateway;

Console.Title = "RemoteSite";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.RemoteSite");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region RemoteSiteGatewayConfig
var gatewayConfig = endpointConfiguration.Gateway(new NonDurableDeduplicationConfiguration());
gatewayConfig.AddReceiveChannel("http://localhost:25899/RemoteSite/");
#endregion

Console.WriteLine("Press any key, application loading");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
