using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageMutators";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.MessageMutators");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        #region ComponentRegistartion
        configure.Configurer.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
        configure.Configurer.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
        #endregion

        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Runner.Run(bus);
        }
    }
}