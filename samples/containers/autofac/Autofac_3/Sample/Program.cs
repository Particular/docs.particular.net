using System;
using Autofac;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Autofac";
        #region ContainerConfiguration
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Autofac");
        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        configure.AutofacBuilder(container);
        #endregion
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}