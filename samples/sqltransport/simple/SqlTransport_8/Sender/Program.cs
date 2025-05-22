using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "SimpleSender";

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

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<InputLoop>();

var host = builder.Build();

await host.RunAsync();

sealed class InputLoop(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Press [c] to send a command, or [e] to publish an event. Press CTRL+C to exit.");
        while (true)
        {
            if (!Console.KeyAvailable)
            {
                await Task.Delay(100, stoppingToken);
                continue;
            }

            var input = Console.ReadKey();
            Console.WriteLine();

            switch (input.Key)
            {
                case ConsoleKey.C:
                    await messageSession.Send(new MyCommand(), cancellationToken: stoppingToken);
                    break;
                case ConsoleKey.E:
                    await messageSession.Publish(new MyEvent(), cancellationToken: stoppingToken);
                    break;
            }
        }
    }
}