using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.FullDuplex.Client";
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.FullDuplex.Client");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            #region ClientLoop

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                Guid guid = Guid.NewGuid();
                Console.WriteLine("Requesting to get data by id: {0}", guid.ToString("N"));

                RequestDataMessage message = new RequestDataMessage
                {
                    DataId = guid,
                    String = "String property value"
                };
                bus.Send("Samples.FullDuplex.Server", message);
            }

            #endregion
        }
    }
}