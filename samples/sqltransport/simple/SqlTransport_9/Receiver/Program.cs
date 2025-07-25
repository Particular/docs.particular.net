using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

Console.Title = "SimpleReceiver";

// Configure the NServiceBus endpoint for the receiver
var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleReceiver");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

// Example connection strings for different SQL Server setups
// for SqlExpress:
//var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=SqlServerSimple;Integrated Security=True;Max Pool Size=100;Encrypt=false";
// for SqlServer:
//var connectionString = @"Server=localhost,1433;Initial Catalog=SqlServerSimple;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
// for LocalDB:
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SqlServerSimple;Integrated Security=True;Connect Timeout=30;Max Pool Size=100;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

// Configure SQL Server transport
endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

// Ensure the database exists before starting
await SqlHelper.EnsureDatabaseExists(connectionString);

// Set up the generic host and register NServiceBus
var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

// Get the required services from the host
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Get the application stopping token to handle graceful shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press CTRL+C to exit");

// Run the host and listen for messages until cancellation is requested
await host.RunAsync(ct);