using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefaultBuilder();
        configure.RijndaelEncryptionService();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
        Console.WriteLine("Press any key to exit");
        Console.ReadLine();
    }
}