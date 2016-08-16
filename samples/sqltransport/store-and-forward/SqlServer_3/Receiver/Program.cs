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
        Console.Title = "Samples.SqlServer.StoreAndForwardReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.StoreAndForwardReceiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(async transportAddress =>
        {
            var connectionString = transportAddress.StartsWith("Samples.SqlServer.StoreAndForwardReceiver") || transportAddress == "error"
                ? ReceiverConnectionString
                : SenderConnectionString;

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync()
                .ConfigureAwait(false);
            return connection;
        });


        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.Recoverability().Delayed(delayed => delayed.NumberOfRetries(0));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for Order messages from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=receiver;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=sender;Integrated Security=True";
}