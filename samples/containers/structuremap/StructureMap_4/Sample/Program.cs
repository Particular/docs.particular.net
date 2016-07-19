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

        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.StructureMap");
        var container = new Container(
            action: expression =>
            {
                expression.For<MyService>()
                    .Use(new MyService());
            });
        configure.StructureMapBuilder(container);

        #endregion

        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}