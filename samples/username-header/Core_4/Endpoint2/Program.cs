using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.Unicast.Config;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint2";
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.UsernameHeader.Endpoint2");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();

        #region manipulate-principal 

        ConfigUnicastBus unicastBus = configure.UnicastBus();
        unicastBus.RunHandlersUnderIncomingPrincipal(true);
        using (IStartableBus startableBus = unicastBus.CreateBus())
        {
            #endregion

            startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}