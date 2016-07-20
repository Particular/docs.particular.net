using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.FaultTolerance.Client";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.FaultTolerance.Client");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var id = Guid.NewGuid();

                var myMessage = new MyMessage
                {
                    Id = id
                };
                bus.Send("Samples.FaultTolerance.Server", myMessage);

                Console.WriteLine($"Sent a message with id: {id.ToString("N")}");
            }
        }
    }
}