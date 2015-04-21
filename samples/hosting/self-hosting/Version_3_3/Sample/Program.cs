using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
	{
		#region self-hosting

		Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.SelfHosting");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.Sagas();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();

        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

		#endregion

        Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
        bus.SendLocal(new MyMessage());
        Console.ReadKey();
    }
}