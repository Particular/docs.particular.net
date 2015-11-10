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
        busConfiguration.EndpointName("Store.Sales");
        busConfiguration.ApplyCommonConfiguration();
        busConfiguration.SendFailedMessagesTo("error");
        using (IBus bus = await Bus.Create(busConfiguration).StartAsync())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
