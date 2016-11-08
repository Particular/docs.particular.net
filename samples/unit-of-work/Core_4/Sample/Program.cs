using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.UnitOfWork";
        Configure.Features.Disable<SecondLevelRetries>();
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.UnitOfWork");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        #region ComponentRegistartion

        configure.Configurer.ConfigureComponent<CustomManageUnitOfWork>(DependencyLifecycle.InstancePerCall);

        #endregion

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Runner.Run(bus);
        }
    }
}