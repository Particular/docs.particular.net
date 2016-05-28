using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Unobtrusive.Client";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefineEndpointName("Samples.Unobtrusive.Client");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.RijndaelEncryptionService();
        configure.ApplyCustomConventions();

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());

            CommandSender.Start(bus);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}