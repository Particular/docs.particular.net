using System;
using NServiceBus;
using NServiceBus.Persistence;
using Shared;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MsmqToSqlRelay.MsmqPublisher";
        #region publisher-config
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("MsmqPublisher");
        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=PersistenceForMsmqTransport;Integrated Security=True");
        busConfiguration.EnableInstallers();
        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to publish an event");
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
                bus.Publish(new SomethingHappened());
                Console.WriteLine("SomethingHappened Event published");
            }
            #endregion
        }
    }
}
