using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Headers";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Headers");

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
        startableBus.OutgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");
        using (IBus bus = startableBus.Start())
        {
            #endregion

            #region sending
            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, "SendingMessage", "ValueSendingMessage");
            bus.SendLocal(myMessage);
            #endregion
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}