using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomChecks.Monitor3rdParty";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.CustomChecks.Monitor3rdParty");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.JsonSerializer();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}