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
        #region msmqpublisher-config

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EndpointName("MsmqPublisher");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True");

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Start(endpoint);
            Console.WriteLine("Press Enter to publish the SomethingHappened Event");
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static void Start(IBusSession busSession)
    {
        #region PublishLoop

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Press Enter to publish the SomethingHappened Event");

            Guid eventId = Guid.NewGuid();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    busSession.Publish(new SomethingHappened());
                    Console.WriteLine("SomethingHappened Event published");
                    continue;
                default:
                    return;
            }
        }

        #endregion
    }
}