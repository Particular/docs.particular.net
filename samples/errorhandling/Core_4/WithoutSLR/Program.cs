using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ErrorHandling.WithoutSLR";
        #region DisableSLR
        Configure.Features.Disable<NServiceBus.Features.SecondLevelRetries>();
        #endregion
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.ErrorHandling.WithoutSLR");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press enter to send a message that will throw an exception.");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                MyMessage m = new MyMessage
                {
                    Id = Guid.NewGuid()
                };
                bus.SendLocal(m);
            }
        }
    }
}