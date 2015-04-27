using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        #region multi-hosting

        using (IBus bus1 = StartInstance1())
        using (IBus bus2 = StartInstance2())
        {
            Console.WriteLine("\r\nBus instances created and configured; press any key to stop program\r\n");
            Console.ReadKey();
        }

        #endregion
    }

    static IBus StartInstance1()
    {
        #region multi-hosting-assembly-scan

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting-1");
        busConfiguration.AssembliesToScan(AllAssemblies.Matching("Instance1."));
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return Bus.Create(busConfiguration).Start();

        #endregion
    }

    static IBus StartInstance2()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting-2");
        busConfiguration.AssembliesToScan(AllAssemblies.Matching("Instance2."));
        busConfiguration.UseSerialization<XmlSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return Bus.Create(busConfiguration).Start();
    }
}