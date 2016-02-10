using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using Shared;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region publisher-config

        var endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EndpointName("MsmqPublisher");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True");
        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Start(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    
    static void Start(IEndpointInstance busSession)
    {
        Console.WriteLine("Press Enter to publish the SomethingHappened Event");
        Console.WriteLine("Press Ctrl+C to exit");

        #region publisher-loop
        while (Console.ReadLine() != null)
        {
            busSession.Publish(new SomethingHappened());
            Console.WriteLine("SomethingHappened Event published");
        }
        #endregion
    }
}