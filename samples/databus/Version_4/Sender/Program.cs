using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.DataBus.Sender");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        #region ConfigureDataBus
        configure.FileShareDataBus("..\\..\\..\\storage");
        #endregion
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
        bus.Send("Samples.DataBus.Receiver", message);

        #endregion
        Console.WriteLine("Message sent, the payload is stored in: ..\\..\\..\\storage");
    }

    static void SendMessageTooLargePayload(IBus bus)
    {
        #region SendMessageTooLargePayload
        AnotherMessageWithLargePayload message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024 * 1024 * 5] //5MB
        };
        bus.Send("Samples.DataBus.Receiver", message);
        #endregion
    }
}