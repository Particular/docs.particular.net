using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "SqlServer";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.SqlServer");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

#region sqlServerConfig

//for local instance or SqlExpress
//var connectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlSagaFinder;Trusted_Connection=True;MultipleActiveResultSets=true";
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlSagaFinder;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(
    connectionBuilder: () =>
    {
        return new SqlConnection(connectionString);
    });
var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

#endregion

await SqlHelper.EnsureDatabaseExists(connectionString);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'enter' to send a message");
while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var startOrder = new StartOrder
    {
        OrderId = "123"
    };
    await messageSession.SendLocal(startOrder);

    Console.WriteLine($"StartOrder sent: {startOrder.OrderId}");
}

await host.StopAsync();