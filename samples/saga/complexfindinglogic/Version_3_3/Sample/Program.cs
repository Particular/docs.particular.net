using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.ComplexSagaFindingLogic");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.Sagas();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        bus.SendLocal(new StartOrder
        {
            OrderId = "123"
        });
        bus.SendLocal(new StartOrder
        {
            OrderId = "456"
        });

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}