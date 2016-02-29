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
        Console.Title = "Samples.MultiHosting";
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
                    await endpoint1.Send("Samples.MultiHosting.Instance2", new MyMessage());
                    continue;
                }
                if (key.Key == ConsoleKey.D2)
                {
                    await endpoint2.Send("Samples.MultiHosting.Instance1", new MyMessage());
                    continue;
                }
                return;
            }
        }
        finally
        {
            if (endpoint1 != null)
            {
                await endpoint1.Stop();
            }
            if (endpoint2 != null)
            {
                await endpoint2.Stop();
            }
        }

        #endregion
    }

    static async Task<IEndpointInstance> StartInstance1()
    {
        #region multi-hosting-assembly-scan

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

        endpointConfiguration.EndpointName("Samples.MultiHosting.Instance1");
        //Exclude Instance2.dll and, by inference, include all other assemblies
        endpointConfiguration.ExcludeAssemblies("Instance2");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        return await Endpoint.Start(endpointConfiguration);

        #endregion
    }

    static async Task<IEndpointInstance> StartInstance2()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

        endpointConfiguration.EndpointName("Samples.MultiHosting.Instance2");
        endpointConfiguration.ExcludeAssemblies("Instance1");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        return await Endpoint.Start(endpointConfiguration);
    }
}