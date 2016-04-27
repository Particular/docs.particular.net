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
        Console.Title = "Samples.SqlServer.SimpleSender";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleSender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region TransportConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=SqlServerSimple;Integrated Security=True");
        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        await endpoint.Send("Samples.SqlServer.SimpleReceiver", new MyMessage());
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }

}