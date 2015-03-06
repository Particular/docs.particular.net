using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{

    static void Main()
    {

        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Sample.PipelineStream.Receiver");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        configure.SetStreamStorageLocation("..\\..\\..\\storage");
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
        Console.WriteLine("\r\nPress enter key to stop program\r\n");
        Console.Read();
    }
}