using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Versioning.V2Publisher";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Versioning.V2Publisher");
        configure.DefaultBuilder();
        configure.JsonSerializer();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            
            Console.WriteLine("Press enter to publish a message");
            Console.WriteLine("Press any key to exit");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                bus.Publish<V2.Messages.ISomethingHappened>(sh =>
                {
                    sh.SomeData = 1;
                    sh.MoreInfo = "It's a secret.";
                });

                Console.WriteLine("Published event.");
            }
        }
    }
}