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
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Spring");
        var applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
        configure.SpringFrameworkBuilder(applicationContext);
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