using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.StepByStep.Client";
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.StepByStep.Client");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            SendOrder(bus);
        }
    }


    #region SendOrder
    static void SendOrder(IBus bus)
    {

        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            Guid id = Guid.NewGuid();

            PlaceOrder placeOrder = new PlaceOrder
            {
                Product = "New shoes",
                Id = id
            };
            bus.Send("Samples.StepByStep.Server", placeOrder);

            Console.WriteLine("Sent a new PlaceOrder message with id: {0}", id.ToString("N"));

        }

    }
    #endregion
}
