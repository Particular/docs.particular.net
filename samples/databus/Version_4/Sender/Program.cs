using System;
using Messages;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {

        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Sample.DataBus.Sender");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        configure.FileShareDataBus(BasePath);
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        Run(bus);
    }


    static void Run(IBus bus)
    {
        Console.WriteLine("Press 'Enter' to send a large message (>4MB)");
        Console.WriteLine("Press 'E' to send a message that will exceed the limit and throw");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.E)
            {
                SendMessageTooLargePayload(bus);
                continue;
            }

            if (key.Key == ConsoleKey.Enter)
            {
                SendMessageLargePayload(bus);
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