using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.DataBus.Receiver");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UseDataBus<FileShareDataBus>().BasePath(BasePath);
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        using (IBus bus = await Bus.Create(busConfiguration).StartAsync())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}