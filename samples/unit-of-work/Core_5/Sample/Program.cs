using System;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    public static void Main()
    {
        Console.Title = "Samples.UnitOfWork";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.UnitOfWork");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.DisableFeature<SecondLevelRetries>();
        #region ComponentRegistartion

        busConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<CustomManageUnitOfWork>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Runner.Run(bus);
        }
    }
}