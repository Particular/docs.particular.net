using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using NServiceBus;


Console.Title = "MySql";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.MySql");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.EnableInstallers();
#region MySqlConfig

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var password = Environment.GetEnvironmentVariable("MySqlPassword");
if (string.IsNullOrWhiteSpace(password))
{
    throw new Exception("Could not extract 'MySqlPassword' from Environment variables.");
}
var username = Environment.GetEnvironmentVariable("MySqlUserName");
if (string.IsNullOrWhiteSpace(username))
{
    throw new Exception("Could not extract 'MySqlUserName' from Environment variables.");
}
var connection = $"server=localhost;user={username};database=sqlpersistencesample;port=3306;password={password};AllowUserVariables=True;AutoEnlist=false";
persistence.SqlDialect<SqlDialect.MySql>();
persistence.ConnectionBuilder(
    connectionBuilder: () =>
    {
        return new MySqlConnection(connection);
    });
var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

#endregion

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