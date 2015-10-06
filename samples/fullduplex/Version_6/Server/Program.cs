using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Server");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IBus bus = await Bus.Create(busConfiguration).StartAsync())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}