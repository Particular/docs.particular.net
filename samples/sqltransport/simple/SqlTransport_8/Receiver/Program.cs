using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "SimpleReceiver";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleReceiver");

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlServerSimple;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=SqlServerSimple;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

endpointConfiguration.EnableInstallers();

await SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

Console.WriteLine("Press CTRL+C to exit");
await host.RunAsync();