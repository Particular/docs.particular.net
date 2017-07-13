using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MultiHosting";
        #region multi-hosting

        using (var bus1 = StartInstance1())
        using (var bus2 = StartInstance2())
        {
            Console.WriteLine("Press '1' to send a message from Instance1 to Instance2");
            Console.WriteLine("Press '2' to send a message from Instance2 to Instance1");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.D1)
                {
                    bus1.Send("Samples.MultiHosting.Instance2", new MyMessage());
                    continue;
                }
                if (key.Key == ConsoleKey.D2)
                {
                    bus2.Send("Samples.MultiHosting.Instance1", new MyMessage());
                    continue;
                }
                return;
            }
        }

        #endregion
    }

    static IBus StartInstance1()
    {
        #region multi-hosting-assembly-scan

        var busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance1");
        // only include Instance1.dll and dependent assemblies
        busConfiguration.AssembliesToScan(AllAssemblies.Matching("Instance1.").And("Shared"));
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return Bus.Create(busConfiguration).Start();

        #endregion
    }

    static IBus StartInstance2()
    {
        var busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance2");
        busConfiguration.AssembliesToScan(AllAssemblies.Matching("Instance2.").And("Shared"));
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return Bus.Create(busConfiguration).Start();
    }
}