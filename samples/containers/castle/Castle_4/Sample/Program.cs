using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Castle";
        Configure.Serialization.Json();
        #region ContainerConfiguration
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Castle");
        var container = new WindsorContainer();
        var registration = Component.For<MyService>()
            .Instance(new MyService());
        container.Register(registration);
        configure.CastleWindsorBuilder(container);
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