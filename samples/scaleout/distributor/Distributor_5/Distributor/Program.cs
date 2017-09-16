using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Scaleout.Server";
        #region server
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Server");
        busConfiguration.RunMSMQDistributor(withWorker: false);
        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}