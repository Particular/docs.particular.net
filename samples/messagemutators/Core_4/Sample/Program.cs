using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageMutators";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.MessageMutators");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        #region ComponentRegistration

        var configurer = configure.Configurer;
        configurer.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
        configurer.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);

        #endregion

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Runner.Run(bus);
        }
    }
}