using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;
using Endpoint;

Console.Title = "InjectingServices";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.InjectingServices");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesInjectedServices;Integrated Security=True;Encrypt=false
//var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesInjectedServices;User Id=SA;Password=yourStrong(!)Password;Encrypt=false;Max Pool Size=100";
// for LocalDB (default choice for development):
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NsbSamplesInjectedServices;Integrated Security=True;Connect Timeout=30;Max Pool Size=100;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

// Configure SQL Server transport
endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

// Configure SQL Server persistence
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(() => new SqlConnection(connectionString));

var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

#region BehaviorConfig

endpointConfiguration.Pipeline.Register(typeof(SqlConnectionBehavior),
    "Forwards the NServiceBus SqlConnection/SqlTransaction to data services injected into message handlers.");

#endregion

#region DependencyInjectionConfig

endpointConfiguration.RegisterComponents(services =>
{
    services.AddScoped<ConnectionHolder>();
    services.AddScoped<IDataService, DataService>();
});

#endregion

// Ensure the database exists before starting the endpoint
SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

// Get the required services from the host
var messageSession = host.Services.GetRequiredService<IMessageSession>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Get the application stopping cancellation token to handle graceful shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press [S] to send a message, or [CTRL+C] to exit.");
while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // Wait a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var key = Console.ReadKey(true);
    Console.WriteLine();

    switch (key.Key)
    {
        case ConsoleKey.S:
            // Send a command message
            logger.LogInformation("Sending command...");
            await messageSession.SendLocal(new TestMsg { Id = Guid.NewGuid() });
            logger.LogInformation("Command sent successfully");
            break;
    }
}

// Ensure the host is stopped gracefully
logger.LogInformation("Stopping host...");
await host.StopAsync();
logger.LogInformation("Host stopped successfully");