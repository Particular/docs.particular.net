using System;
using System.Security.Principal;
using System.Threading;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.UsernameHeader.Endpoint1");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();

        #region ComponentRegistartion
        configure.Configurer
            .ConfigureComponent<UsernameMutator>(DependencyLifecycle.InstancePerCall);
        #endregion

        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            #region SendMessage
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("FakeUser"), new string[0]);
            bus.Send("Samples.UsernameHeader.Endpoint2", new MyMessage());
            #endregion
            Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
            Console.ReadKey();
        }
    }
}