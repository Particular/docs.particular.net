using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.FullDuplex.Server";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.FullDuplex.Server");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install()); 
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}