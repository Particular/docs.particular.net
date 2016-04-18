using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
	{
        Console.Title = "Samples.SelfHosting";
		#region self-hosting

        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.SelfHosting");
        configure.DefaultBuilder();
        configure.Sagas();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();

        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            bus.SendLocal(new MyMessage());
            Console.ReadKey();
        }

        #endregion

    }
}