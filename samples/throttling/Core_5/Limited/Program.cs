using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Throttling.Limited";

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Throttling.Limited");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #region RegisterBehavior

        busConfiguration.Pipeline.Register<ThrottlingRegistration>();

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}