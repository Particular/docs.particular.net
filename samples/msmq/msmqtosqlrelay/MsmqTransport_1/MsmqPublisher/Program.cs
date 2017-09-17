using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using Shared;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MsmqToSqlRelay.MsmqPublisher";
        #region publisher-config

        var endpointConfiguration = new EndpointConfiguration("MsmqPublisher");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=PersistenceForMsmqTransport;Integrated Security=True");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Start(endpointInstance);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static void Start(IEndpointInstance busSession)
    {
        Console.WriteLine("Press Enter to publish the SomethingHappened Event");
        Console.WriteLine("Press any key to exit");

        #region publisher-loop
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            busSession.Publish(new SomethingHappened());
            Console.WriteLine("SomethingHappened Event published");
        }
        #endregion
    }
}