using NServiceBus;

class Program
{
    public static void Main()
    {
        #region ComponentRegistartion
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageMutators");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Runner.Run(bus);
        }
    }
}