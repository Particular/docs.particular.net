using System;
using Messages;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Sample.DataBus.Sender");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        configure.FileShareDataBus(BasePath);
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press 'D' to send a databus large message");
            Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
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
        MessageWithLargePayload message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new DataBusProperty<byte[]>(new byte[1024 * 1024 * 5]) //5MB
        };
        bus.Send("Sample.DataBus.Receiver", message);

        #endregion
        Console.WriteLine("Message sent, the payload is stored in: " + BasePath);
    }

    static void SendMessageTooLargePayload(IBus bus)
    {
        #region SendMessageTooLargePayload
        AnotherMessageWithLargePayload message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024 * 1024 * 5] //5MB
        };
        bus.Send("Sample.DataBus.Receiver", message);
        #endregion
    }
}