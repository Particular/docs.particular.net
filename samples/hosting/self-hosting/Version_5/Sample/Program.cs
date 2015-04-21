using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        #region self-hosting

        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.SelfHosting");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();

            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            bus.SendLocal(new MyMessage());
            Console.ReadKey();
        }

        #endregion
    }
}