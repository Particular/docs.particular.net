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

        #region ComponentRegistration

        busConfiguration.RegisterComponents(
            registration: components =>
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