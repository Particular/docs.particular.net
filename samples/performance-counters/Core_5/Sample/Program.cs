using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.PerfCounters";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PerfCounters");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #region enable-counters
        busConfiguration.EnableCriticalTimePerformanceCounter();
        busConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromSeconds(100));
        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {

            Console.WriteLine("Press enter to send 10 messages with random sleep");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                for (var i = 0; i < 10; i++)
                {
                    var myMessage = new MyMessage();
                    bus.SendLocal(myMessage);
                }
            }
        }
    }
}
