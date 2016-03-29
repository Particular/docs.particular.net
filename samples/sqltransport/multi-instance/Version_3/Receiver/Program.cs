using System;
using System.Threading.Tasks;
using NServiceBus;
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
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .EnableLagacyMultiInstanceMode(ConnectionProvider.GetConnection);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for Order messages from the Sender");
        Console.ReadKey();
        await endpoint.Stop();
    }

}