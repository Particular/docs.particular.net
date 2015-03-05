using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {

        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Sample.DataBus.Receiver");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        configure.FileShareDataBus(BasePath);
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
        Console.WriteLine("\r\nPress enter key to stop program\r\n");
        Console.Read();
    }
}