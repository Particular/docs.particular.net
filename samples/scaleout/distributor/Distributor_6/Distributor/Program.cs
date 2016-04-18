using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Scaleout.Server";
        #region server
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Server");
        busConfiguration.RunMSMQDistributor(withWorker: false);
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}