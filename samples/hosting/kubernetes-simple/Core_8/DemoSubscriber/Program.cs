using System.Data.SqlClient;
using NServiceBus;

var sqlConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");


var config = new EndpointConfiguration("KubernetesDemo.Subscriber");

config.EnableInstallers();

var transport = new SqlServerTransport(sqlConnectionString);
transport.Subscriptions.DisableCaching = true;
config.UseTransport(transport);

var persistence = config.UsePersistence<SqlPersistence>();
persistence.ConnectionBuilder(() => new SqlConnection(sqlConnectionString));
persistence.SqlDialect<SqlDialect.MsSqlServer>();

config.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));

var endpoint = await Endpoint.Start(config);
Console.WriteLine("Endpoint started");

while (true)
{
    if (Console.IsInputRedirected)
    {
        await Task.Delay(10000);
        continue;
    }

    Console.WriteLine("Press [Esc] to exit");
    var key = Console.ReadKey();
    if (key.KeyChar == (int)ConsoleKey.Escape)
    {
        break;
    }


    Console.WriteLine();
}

await endpoint.Stop();
