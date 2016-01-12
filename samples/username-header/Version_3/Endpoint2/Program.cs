using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.Unicast.Config;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.UsernameHeader.Endpoint2");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        #region manipulate-principal 
        ConfigUnicastBus unicastBus = configure.UnicastBus();
        unicastBus.ImpersonateSender(true);
        using (IStartableBus startableBus = unicastBus.CreateBus())
        {
            #endregion
            startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}