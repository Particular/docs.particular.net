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
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Castle"); 
        WindsorContainer container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));
        configure.CastleWindsorBuilder(container);
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