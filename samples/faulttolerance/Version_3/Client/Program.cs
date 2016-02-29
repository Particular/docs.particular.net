using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.FaultTolerance.Client";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.FaultTolerance.Client");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                Guid id = Guid.NewGuid();

                bus.Send("Samples.FaultTolerance.Server", new MyMessage
                {
                    Id = id
                });

                Console.WriteLine("Sent a new message with id: {0}", id.ToString("N"));
            }
        }
    }
}