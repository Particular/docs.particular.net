using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.PerfCounters";
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.PerfCounters");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();

        #region enable-counters
        configure.EnablePerformanceCounters();
        configure.SetEndpointSLA(TimeSpan.FromSeconds(100));
        #endregion

        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            Console.WriteLine("Press enter to send 10 messages with random sleep");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                for (int i = 0; i < 10; i++)
                {
                    bus.SendLocal(new MyMessage());
                }
            }
        }

    }
}