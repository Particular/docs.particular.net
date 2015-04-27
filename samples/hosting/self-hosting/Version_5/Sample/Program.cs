using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        #region self-hosting

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SelfHosting");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();

            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            bus.SendLocal(new MyMessage());
            Console.ReadKey();
        }

        #endregion
    }
}