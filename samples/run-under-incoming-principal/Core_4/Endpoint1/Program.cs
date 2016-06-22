using System;
using System.Security.Principal;
using System.Threading;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.RunUnderIncomingPrincipal.Endpoint1";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.RunUnderIncomingPrincipal.Endpoint1");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            #region SendMessage
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("FakeUser"), new string[0]);
            bus.Send("Samples.RunUnderIncomingPrincipal.Endpoint2", new MyMessage());
            #endregion

            Console.WriteLine("Message sent. Press any key to exit");
            Console.ReadKey();
        }
    }
}