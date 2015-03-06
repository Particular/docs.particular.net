using System;
using System.IO;
using Messages;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
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
        configure.SetStreamStorageLocation("..\\..\\..\\storage");
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        Run(bus);
    }


    static void Run(IBus bus)
    {
        Console.WriteLine("Press 'Enter' to send a message with a stream");
        Console.WriteLine("Press 'E' to send a message that will exceed the limit and throw");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.Enter)
            {
                SendMessageLargePayload(bus);
            }
        }
    }

    static void SendMessageLargePayload(IBus bus)
    {
        #region SendMessageLargePayload
        MessageWithStream message = new MessageWithStream
        {
            SomeProperty = "This message contains a large stream that will be written to a file share",
            LargeStream = File.OpenRead("FileToSend.txt")

        };
        bus.Send("Sample.DataBus.Receiver", message);

        #endregion
        Console.WriteLine("Message sent");
    }

}