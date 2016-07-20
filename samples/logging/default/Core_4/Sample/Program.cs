using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Logging.Default";
        Configure.Serialization.Json();
        #region ConfigureLogging
        var configure = Configure.With();
        // Configures a ConsoleAppender with a threshold of Debug
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Logging.Default");
        #endregion
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}