using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ComplexSagaFindingLogic";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.ComplexSagaFindingLogic");
        configure.DefaultBuilder();
        configure.Sagas();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
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

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}