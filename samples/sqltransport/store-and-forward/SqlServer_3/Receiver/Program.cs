using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;
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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.StoreAndForwardReceiver");
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        endpointConfiguration.UseTransport<SqlServerTransport>()
            .EnableLagacyMultiInstanceMode(async transportAddress =>
            {
                string connectionString = transportAddress.StartsWith("Samples.SqlServer.StoreAndForwardReceiver") || transportAddress == "error"
                    ? ReceiverConnectionString
                    : SenderConnectionString;

                SqlConnection connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return connection;
            });


        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.WriteLine("Waiting for Order messages from the Sender");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=receiver;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True";
}