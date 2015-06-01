using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Versioning.V1Subscriber");
        configure.DefaultBuilder();
        configure.JsonSerializer();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.UnicastBus()
            .CreateBus()
            .Start(() => configure.ForInstallationOn<Windows>().Install());

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}