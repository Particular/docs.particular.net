using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using StructureMap;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.StructureMap";
        #region ContainerConfiguration
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.StructureMap");
        Container container = new Container(x => x.For<MyService>().Use(new MyService()));
        configure.StructureMapBuilder(container);
        #endregion
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}