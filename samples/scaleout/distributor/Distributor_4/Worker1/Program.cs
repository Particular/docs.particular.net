using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Scaleout.Worker1";
        Configure.Serialization.Json();

#pragma warning disable 618
        #region Workerstartup
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Scaleout.Worker1");
        configure.DefaultBuilder();
        configure.EnlistWithDistributor();
        #endregion
#pragma warning restore 618

        Address.InitializeLocalAddress("Samples.Scaleout.Worker1");
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}