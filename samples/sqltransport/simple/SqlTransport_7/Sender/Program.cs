using System;
using System.Threading.Tasks;
using NServiceBus;

Console.Title = "Samples.SqlServer.SimpleSender";
var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleSender");
endpointConfiguration.EnableInstallers();

#region TransportConfiguration
// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlServerSimple;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=SqlServerSimple;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
var routing = endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

routing.RouteToEndpoint(typeof(MyCommand), "Samples.SqlServer.SimpleReceiver");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

await SqlHelper.EnsureDatabaseExists(connectionString);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

await SendMessages(endpointInstance);

await endpointInstance.Stop();

static async Task SendMessages(IMessageSession messageSession)
{
    Console.WriteLine("Press [c] to send a command, or [e] to publish an event. Press [Esc] to exit.");
    while (true)
    {
        var input = Console.ReadKey();
        Console.WriteLine();

        switch (input.Key)
        {
            case ConsoleKey.C:
                await messageSession.Send(new MyCommand());
                break;
            case ConsoleKey.E:
                await messageSession.Publish(new MyEvent());
                break;
            case ConsoleKey.Escape:
                return;
        }
    }
}

