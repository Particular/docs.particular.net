using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        Configure.Features.Enable<Sagas>();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.ComplexSagaFindingLogic");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
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