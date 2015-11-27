using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PerfCounters");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        #region enable-counters
        busConfiguration.EnableCriticalTimePerformanceCounter();
        busConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromSeconds(100));
        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();

            Console.WriteLine("Press enter to send 10 messages with random sleep");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }
                for (int i = 0; i < 10; i++)
                {
                    await busContext.SendLocal(new MyMessage());
                }
            }
        }
        finally
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
    }
}
