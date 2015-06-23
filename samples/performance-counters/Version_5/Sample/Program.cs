using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PerfCounters");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #region enable-counters
        busConfiguration.EnableCriticalTimePerformanceCounter();
        busConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromSeconds(100));
        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to send 10 messages with random sleep");

            Console.WriteLine("To exit, press Escape");
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                for (int i = 0; i < 10; i++)
                {
                    bus.SendLocal(new MyMessage());
                }
            }
        }
    }
}
