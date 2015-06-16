using System;
using Ninject;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();

        #region ContainerConfiguration

        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Ninject");
        StandardKernel kernel = new StandardKernel();
        kernel.Bind<MyService>().ToConstant(new MyService());
        configure.NinjectBuilder(kernel);

        #endregion

        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}