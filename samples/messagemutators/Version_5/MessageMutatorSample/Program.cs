using NServiceBus;

class Program
{
    public static void Main()
    {
        #region ComponentRegistartion
        var busConfiguration = new BusConfiguration();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        using (var bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Runner.Run(bus);
        }
    }
}