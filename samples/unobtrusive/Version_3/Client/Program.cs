using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Unobtrusive.Client";
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Unobtrusive.Client");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.ApplyCustomConventions();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        configure.RijndaelEncryptionService();
  
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            CommandSender.Start(bus);
        }
    }
}