using System;
using System.Threading.Tasks;
using NServiceBus;
using Store.Shared;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.ContentManagement");
        busConfiguration.ApplyCommonConfiguration();
        using (IBus bus = await Bus.Create(busConfiguration).StartAsync())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
