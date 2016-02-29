using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Gateway.RemoteSite";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Gateway.RemoteSite");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.RunGatewayWithInMemoryPersistence();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}