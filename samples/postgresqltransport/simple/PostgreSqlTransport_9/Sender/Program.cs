using System;
using System.Threading.Tasks;
using NServiceBus;

Console.Title = "SimpleSender";
var endpointConfiguration = new EndpointConfiguration("PostgreSql.SimpleSender");
endpointConfiguration.EnableInstallers();

#region TransportConfiguration
var connectionString = "User ID=user;Password=admin;Host=localhost;Port=54320;Database=nservicebus;Pooling=true;Connection Lifetime=0;Include Error Detail=true";
var routing = endpointConfiguration.UseTransport(new PostgreSqlTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

routing.RouteToEndpoint(typeof(MyCommand), "PostgreSql.SimpleReceiver");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

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

