using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        #region self-hosting

        Configure.Serialization.Json();
        Configure.Features.Enable<Sagas>();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.SelfHosting");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        #endregion

        Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
        bus.SendLocal(new MyMessage());
        Console.ReadKey();
    }
}