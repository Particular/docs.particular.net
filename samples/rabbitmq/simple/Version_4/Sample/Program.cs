using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        #region ConfigureRabbit
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.RabbitMQ.Simple");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.UseTransport<NServiceBus.RabbitMQ>(() => "host=localhost");
        #endregion
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        bus.SendLocal(new MyMessage());

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }

}