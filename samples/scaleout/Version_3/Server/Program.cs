using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        #region server
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Scaleout.Server");
        configure.DefaultBuilder();
        configure.RunDistributor(withWorker: false);
        #endregion
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}