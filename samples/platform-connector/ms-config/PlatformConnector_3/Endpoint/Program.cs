using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Endpoint";

var hostBuilder = Host.CreateApplicationBuilder(args);

#region addConfigFile
hostBuilder.Configuration.AddJsonFile("appsettings.json");
#endregion

var endpointConfiguration = new EndpointConfiguration("Endpoint");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UsePersistence<NonDurablePersistence>();

#region loadConnectionDetails
var platformConnection = hostBuilder.Configuration
    .GetSection("ServicePlatformConfiguration")
    .Get<ServicePlatformConnectionConfiguration>();
#endregion

#region configureConnection
endpointConfiguration.ConnectToServicePlatform(platformConnection);
#endregion

hostBuilder.UseNServiceBus(endpointConfiguration);

hostBuilder.Services.AddHostedService<BusinessMessageSimulator>();

await hostBuilder.Build().RunAsync();