using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.TruncateSender";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.TruncateSender");
        endpointConfiguration.EnableInstallers();

        #region TransportConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SQLServerTruncate;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=SQLServerTruncate;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        transport.ConnectionString(connectionString);
        transport.Routing().RouteToEndpoint(typeof(MyCommand), "Samples.SqlServer.TruncateReceiver");

        #endregion

        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        await SqlHelper.EnsureDatabaseExists(connectionString);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await SendMessages(endpointInstance);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

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
}