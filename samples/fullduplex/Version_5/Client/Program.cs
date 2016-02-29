using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.FullDuplex.Client";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
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