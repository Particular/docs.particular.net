using System;
using NServiceBus;

class Program
{
    public static void Main()
    {
        Console.Title = "Samples.MessageMutators";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageMutators");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();

        #region ComponentRegistration
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Runner.Run(bus);
        }
    }
}