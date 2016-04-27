using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.SimpleReceiver";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleReceiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=SqlServerSimple;Integrated Security=True");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.WriteLine("Waiting for message from the Sender");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }

}