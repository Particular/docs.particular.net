using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;


Console.Title = "MySql";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.MySql");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<XmlSerializer>();
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

var endpointInstance = await Endpoint.Start(endpointConfiguration);
var startOrder = new StartOrder
{
    OrderId = "123"
};
await endpointInstance.SendLocal(startOrder);

Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();