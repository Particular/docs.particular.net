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

        IEndpointInstance endpoint1 = null;
        IEndpointInstance endpoint2 = null;
        try
        {
            endpoint1 = await StartInstance1();
            endpoint2 = await StartInstance2();

            Console.WriteLine("Press '1' to send a message from Instance1 to Instance2");
            Console.WriteLine("Press '2' to send a message from Instance2 to Instance1");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.D1)
                {
                    IBusSession busSession1 = endpoint1.CreateBusSession();
                    await busSession1.Send("Samples.MultiHosting.Instance2", new MyMessage());
                    continue;
                }
                if (key.Key == ConsoleKey.D2)
                {
                    IBusSession busSession2 = endpoint2.CreateBusSession();
                    await busSession2.Send("Samples.MultiHosting.Instance1", new MyMessage());
                    continue;
                }
                return;
            }
        }
        finally
        {
            if (endpoint1 != null)
            {
                endpoint1.Stop().GetAwaiter().GetResult();
            }
            if (endpoint2 != null)
            {
                endpoint2.Stop().GetAwaiter().GetResult();
            }
        }

        #endregion
    }

    static async Task<IEndpointInstance> StartInstance1()
    {
        #region multi-hosting-assembly-scan

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance1");
        //Exclude Instance2.dll and, by inference, include all other assemblies
        busConfiguration.ExcludeAssemblies("Instance2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        return await Endpoint.Start(busConfiguration);

        #endregion
    }

    static async Task<IEndpointInstance> StartInstance2()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance2");
        busConfiguration.ExcludeAssemblies("Instance1");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        return await Endpoint.Start(busConfiguration);
    }
}