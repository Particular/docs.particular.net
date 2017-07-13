using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.NHibernateCustomSagaFinder";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.NHibernateCustomSagaFinder");
        busConfiguration.EnableInstallers();

        #region config

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=NHCustomSagaFinder;Integrated Security=True");

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var startOrder = new StartOrder
            {
                OrderId = "123"
            };
            bus.SendLocal(startOrder);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
