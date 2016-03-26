using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
#pragma warning disable 618

class Program
{
    static IEndpointInstance endpoint;

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .EnableLagacyMultiInstanceMode(ConnectionProvider.GetConnecton);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #endregion

        endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Receiver running. Press <enter> key to quit");
        Console.WriteLine("Waiting for Order messages from the Sender");

        while (true)
        {
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                await endpoint.Stop();
                break;
            }
        }
    }

}