using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.ObjectBuilder;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Headers");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        IConfigureComponents components = configure.Configurer;
        components.ConfigureComponent<MutateIncomingMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateIncomingTransportMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateOutgoingMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateOutgoingTransportMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MessageMutator>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateTransportMessages>(DependencyLifecycle.InstancePerCall);
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }

    }
}