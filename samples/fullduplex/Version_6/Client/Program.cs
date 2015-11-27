using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
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
                await busContext.Send("Samples.FullDuplex.Server", message);
            }

            #endregion
        }
        finally
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
    }
}