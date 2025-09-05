using Microsoft.Extensions.Hosting;


Console.Title = "Server";
var builder = Host.CreateApplicationBuilder(args);

#region mongoDbConfig

var endpointConfiguration = new EndpointConfiguration("Samples.MongoDB.Server");
var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
persistence.DatabaseName("Samples_MongoDB_Server");

#endregion

endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();