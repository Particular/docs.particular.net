using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Callbacks.Receiver";
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Callbacks.Receiver");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}