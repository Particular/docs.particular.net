using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.MessageMutators");
        configure.Log4Net();
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
            IBus bus = startableBus.Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
            Runner.Run(bus);
        }
    }
}