using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefineEndpointName("Samples.Versioning.V2Publisher");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        var bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        Console.WriteLine("Press 'Enter' to publish a message, Ctrl + C to exit.");

        while (Console.ReadLine() != null)
        {
            bus.Publish<V2.Messages.ISomethingHappened>(sh =>
            {
                sh.SomeData = 1;
                sh.MoreInfo = "It's a secret.";
            });

            Console.WriteLine("Published event.");
        }
    }
}