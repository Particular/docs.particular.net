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
            endpoint1 = await StartInstance1()
                .ConfigureAwait(false);
            endpoint2 = await StartInstance2()
                .ConfigureAwait(false);

            Console.WriteLine("Press '1' to send a message from Instance1 to Instance2");
            Console.WriteLine("Press '2' to send a message from Instance2 to Instance1");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                var message = new MyMessage();
                if (key.Key == ConsoleKey.D1)
                {
                    await endpoint1.Send("Samples.MultiHosting.Instance2", message)
                        .ConfigureAwait(false);
                    continue;
                }
                if (key.Key == ConsoleKey.D2)
                {
                    await endpoint2.Send("Samples.MultiHosting.Instance1", message)
                        .ConfigureAwait(false);
                    continue;
                }
                return;
            }
        }
        finally
        {
            if (endpoint1 != null)
            {
                await endpoint1.Stop()
                    .ConfigureAwait(false);
            }
            if (endpoint2 != null)
            {
                await endpoint2.Stop()
                    .ConfigureAwait(false);
            }
        }

        #endregion
    }

    static Task<IEndpointInstance> StartInstance1()
    {
        #region multi-hosting-assembly-scan

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiHosting.Instance1");
        // Exclude Instance2.dll and, by inference, include all other assemblies
        var assemblyScanner = endpointConfiguration.AssemblyScanner();
        assemblyScanner.ExcludeAssemblies("Instance2");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        return Endpoint.Start(endpointConfiguration);

        #endregion
    }

    static Task<IEndpointInstance> StartInstance2()
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.MultiHosting.Instance2");
        var assemblyScanner = endpointConfiguration.AssemblyScanner();
        assemblyScanner.ExcludeAssemblies("Instance1");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        return Endpoint.Start(endpointConfiguration);
    }
}