using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        var configure = Configure.With();
        configure.DefineEndpointName("EncryptionSampleEndpoint2");
        configure.DefaultBuilder();
        configure.RijndaelEncryptionService();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
        Console.WriteLine("Press any key to exit");
        Console.ReadLine();
    }
}