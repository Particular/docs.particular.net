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
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Client");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            #region ClientLoop

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var guid = Guid.NewGuid();
                Console.WriteLine($"Requesting to get data by id: {guid.ToString("N")}");

                var message = new RequestDataMessage
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