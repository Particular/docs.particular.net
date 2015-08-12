using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Versioning.V2Subscriber");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}