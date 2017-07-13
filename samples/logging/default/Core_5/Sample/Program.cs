using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.Default";
        #region ConfigureLogging
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Default");
        // No config is required in version 5 and 
        // higher since logging is enabled by default
        #endregion
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
