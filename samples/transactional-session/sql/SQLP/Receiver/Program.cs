using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

Console.Title = "Receiver";

var builder = Host.CreateApplicationBuilder(args);

const string ConnectionString = @"";

var endpointConfiguration = new EndpointConfiguration("Receiver");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseTransport(new SqlServerTransport(ConnectionString));

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();