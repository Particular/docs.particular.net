using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.DataBus.Sender";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.DataBus.Sender");
        #region ConfigureDataBus
        var dataBus = busConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath(@"..\..\..\storage");
        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'D' to send a databus large message");
            Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.N)
                {
                    SendMessageTooLargePayload(bus);
                    continue;
                }

                if (key.Key == ConsoleKey.D)
                {
                    SendMessageLargePayload(bus);
                    continue;
                }
                return;
            }
        }
    }


    static void SendMessageLargePayload(IBus bus)
    {
        #region SendMessageLargePayload
        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new DataBusProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        bus.Send("Samples.DataBus.Receiver",message);

        #endregion
        Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
    }

    static void SendMessageTooLargePayload(IBus bus)
    {
        #region SendMessageTooLargePayload
        var message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024 * 1024 * 5] //5MB
        };
        bus.Send("Samples.DataBus.Receiver", message);
        #endregion
    }
}