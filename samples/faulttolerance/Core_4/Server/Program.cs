using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        #region disable
        // Configure.Features.Disable<NServiceBus.Features.SecondLevelRetries>();
        #endregion

        Console.Title = "Samples.FaultTolerance.Server";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.FaultTolerance.Server");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}