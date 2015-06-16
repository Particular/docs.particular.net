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
        configure.Log4Net();
        configure.DefineEndpointName("Samples.ComplexSagaFindingLogic");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

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
}