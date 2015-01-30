using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefineEndpointName("Samples.Mvc.Server");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        var bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}