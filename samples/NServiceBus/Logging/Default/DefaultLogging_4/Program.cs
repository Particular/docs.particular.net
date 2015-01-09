using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        var configure = Configure.With();

        //Configures a ConsoleAppender with a threshold of Debug
        //Note that log4net.dll needs to exist in the bin directory
        configure.Log4Net();

        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        var bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        bus.SendLocal(new MyMessage());

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}