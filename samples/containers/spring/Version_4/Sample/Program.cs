using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using Spring.Context.Support;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Spring";
        Configure.Serialization.Json();
        #region ContainerConfiguration
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Spring");
        GenericApplicationContext applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
        configure.SpringFrameworkBuilder(applicationContext);
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