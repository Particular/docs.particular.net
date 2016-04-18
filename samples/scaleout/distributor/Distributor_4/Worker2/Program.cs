using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Scaleout.Worker2";
        Configure.Serialization.Json();

#pragma warning disable 618
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Scaleout.Worker2");
        configure.DefaultBuilder();
        configure.EnlistWithDistributor();
#pragma warning restore 618

        #region WorkerNameToUseWhileTestingCode
        //called after EnlistWithDistributor
        Address.InitializeLocalAddress("Samples.Scaleout.Worker2");
        #endregion
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
