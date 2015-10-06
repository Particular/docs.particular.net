using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncRun().GetAwaiter().GetResult();
    }

    static async Task AsyncRun()
    {
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
        });

        #region global-all-outgoing

        busConfiguration.AddHeaderToAllOutgoingMessages("AllOutgoing", "ValueAllOutgoing");
        IStartableBus startableBus = Bus.Create(busConfiguration);
        using (IBus bus = await startableBus.StartAsync())
        {
            #endregion

            #region sending

            MyMessage myMessage = new MyMessage();
            await bus.SendLocalAsync(myMessage);

            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}