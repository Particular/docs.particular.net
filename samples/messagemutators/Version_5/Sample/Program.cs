using System;
using NServiceBus;

class Program
{
    public static void Main()
    {
        Console.Title = "Samples.MessageMutators";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageMutators");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();

        #region ComponentRegistartion
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Runner.Run(bus);
        }
    }
}