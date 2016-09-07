using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SQLServer;
#pragma warning disable 618

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(ConnectionProvider.GetConnection);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for Order messages from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static class ConnectionProvider
    {
        const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True";
        const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True";

        public static async Task<SqlConnection> GetConnection(string transportAddress)
        {
            var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender")
                ? SenderConnectionString
                : DefaultConnectionString;

            var connection = new SqlConnection(connectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            return connection;
        }
    }
}