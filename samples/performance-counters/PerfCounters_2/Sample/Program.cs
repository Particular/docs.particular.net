using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.PerfCounters";
        var endpointConfiguration = new EndpointConfiguration("Samples.PerfCounters");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region enable-counters
        var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
        performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromSeconds(100));
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send 10 messages with random sleep");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            for (var i = 0; i < 10; i++)
            {
                var myMessage = new MyMessage();
                await endpointInstance.SendLocal(myMessage)
                    .ConfigureAwait(false);
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
