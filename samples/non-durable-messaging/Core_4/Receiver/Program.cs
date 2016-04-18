using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageDurability.Receiver";
        Configure.Serialization.Json();
        Configure.Transactions.Disable();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.MessageDurability.Receiver");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}