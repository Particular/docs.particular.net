using System;
using NServiceBus;

class Program
{

    static void Main()
    {

        #region ConfigureLogging
        var busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.Logging.Default");
        //Note that no config is required in V5 since logging is enabled by default
        #endregion
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfig))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
