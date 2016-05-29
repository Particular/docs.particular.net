using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SelfHosting";
        #region self-hosting

        Configure.Serialization.Json();
        Configure.Features.Enable<Sagas>();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.SelfHosting");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.ReadKey();
        }

        #endregion

    }
}