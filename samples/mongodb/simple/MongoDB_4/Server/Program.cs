using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;


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
Console.ReadKey();
builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();