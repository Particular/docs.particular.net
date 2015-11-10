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
        #region ConfigureLogging
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Default");
        //Note that no config is required in version 5 and higher since logging is enabled by default
        #endregion
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = await Bus.Create(busConfiguration).StartAsync())
        {
            await bus.SendLocalAsync(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
