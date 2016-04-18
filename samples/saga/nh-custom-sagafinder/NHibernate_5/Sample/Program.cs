using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.NHibernateCustomSagaFinder";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.NHibernateCustomSagaFinder");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        #region NHibernateSetup

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SqlExpress;Initial Catalog=NHCustomSagaFinder;Integrated Security=True");

        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new StartOrder
                          {
                              OrderId = "123"
                          });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
