using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main(string[] args)
    {
        Configure configure = Configure.With();
        Configure.Serialization.Json();

        configure.Log4Net();
        configure.DefineEndpointName("Samples.Unobtrusive.Server");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.RijndaelEncryptionService();
        configure.ApplyCustomConventions();

        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());

            CommandSender.Start(bus);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}