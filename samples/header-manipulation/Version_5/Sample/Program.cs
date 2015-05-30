using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Default");

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<MutateIncomingMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateIncomingTransportMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateOutgoingMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateOutgoingTransportMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MessageMutator>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateTransportMessages>(DependencyLifecycle.InstancePerCall);
        });

        #region global-all-outgoing

        IStartableBus startableBus = Bus.Create(busConfiguration);
        startableBus.OutgoingHeaders.Add("KeyForAllOutgoing", "ValueForAllOutgoing");
        using (IBus bus = startableBus.Start())
        {
            #endregion

            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}