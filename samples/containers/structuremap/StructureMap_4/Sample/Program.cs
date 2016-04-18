using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using StructureMap;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.StructureMap";
        Configure.Serialization.Json();

        #region ContainerConfiguration

        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.StructureMap");
        Container container = new Container(x => x.For<MyService>().Use(new MyService()));
        configure.StructureMapBuilder(container);

        #endregion

        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}