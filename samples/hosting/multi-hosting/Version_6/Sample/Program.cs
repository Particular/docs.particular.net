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
        #region multi-hosting

        using (IBus bus1 = await StartInstance1())
        using (IBus bus2 = await StartInstance2())
        {
            Console.WriteLine("Press '1' to send a message from Instance1 to Instance2");
            Console.WriteLine("Press '2' to send a message from Instance2 to Instance1");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.D1)
                {
                    await bus1.SendAsync("Samples.MultiHosting.Instance2", new MyMessage());
                    continue;
                }
                if (key.Key == ConsoleKey.D2)
                {
                    await bus2.SendAsync("Samples.MultiHosting.Instance1", new MyMessage());
                    continue;
                }
                return;
            }
        }

        #endregion
    }

    static async Task<IBus> StartInstance1()
    {
        #region multi-hosting-assembly-scan

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance1");
        //Exclude Instance2.dll and, by inference, include all other assemblies
        busConfiguration.ExcludeAssemblies("Instance2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return await Bus.Create(busConfiguration).StartAsync();

        #endregion
    }

    static async Task<IBus> StartInstance2()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance2");
        busConfiguration.ExcludeAssemblies("Instance1");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return await Bus.Create(busConfiguration).StartAsync();
    }
}