using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        var configure = Configure.With();
        configure.DefineEndpointName("Samples.Versioning.V2Subscriber");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}