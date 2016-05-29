using System;
using Ninject;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Ninject";
        Configure.Serialization.Json();

        #region ContainerConfiguration

        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Ninject");
        var kernel = new StandardKernel();
        kernel.Bind<MyService>().ToConstant(new MyService());
        configure.NinjectBuilder(kernel);

        #endregion

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