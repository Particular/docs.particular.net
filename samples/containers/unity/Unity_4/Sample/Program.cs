using System;
using Microsoft.Practices.Unity;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Unity";
        Configure.Serialization.Json();

        #region ContainerConfiguration

        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Unity");
        var container = new UnityContainer();
        container.RegisterInstance(new MyService());
        configure.UnityBuilder(container);

        #endregion

        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            bus.SendLocal(new MyMessage());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}