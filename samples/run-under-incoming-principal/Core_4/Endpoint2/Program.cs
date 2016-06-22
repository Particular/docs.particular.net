using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.RunUnderIncomingPrincipal.Endpoint2";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.RunUnderIncomingPrincipal.Endpoint2");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();

        #region manipulate-principal

        var unicastBus = configure.UnicastBus();
        unicastBus.RunHandlersUnderIncomingPrincipal(true);
        using (var startableBus = unicastBus.CreateBus())
        {
            #endregion

            startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}