using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.DataBus.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UseDataBus<FileShareDataBus>().BasePath(BasePath);
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
            Console.WriteLine("Press 'D' to send a databus large message");
            Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.N)
                {
                    await SendMessageTooLargePayload(busContext);
                    continue;
                }

                if (key.Key == ConsoleKey.D)
                {
                    await SendMessageLargePayload(busContext);
                    continue;
                }
                return;
            }
        }
        finally
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
    }


    static async Task SendMessageLargePayload(IBusContext bus)
    {
        #region SendMessageLargePayload

        MessageWithLargePayload message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new DataBusProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        await bus.Send("Samples.DataBus.Receiver", message);

        #endregion

        Console.WriteLine("Message sent, the payload is stored in: " + BasePath);
    }

    static async Task SendMessageTooLargePayload(IBusContext bus)
    {
        #region SendMessageTooLargePayload

        AnotherMessageWithLargePayload message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024*1024*5] //5MB
        };
        await bus.Send("Samples.DataBus.Receiver", message);

        #endregion
    }
}