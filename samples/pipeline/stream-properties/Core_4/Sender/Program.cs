using System;
using System.IO;
using System.Net;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.PipelineStream.Sender";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.PipelineStream.Sender");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        #region configure-stream-storage

        configure.SetStreamStorageLocation("..\\..\\..\\storage");

        #endregion

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Run(bus);
        }
    }


    static void Run(IBus bus)
    {
        Console.WriteLine("Press 'F' to send a message with a file stream");
        Console.WriteLine("Press 'H' to send a message with a http stream");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.F)
            {
                SendMessageWithFileStream(bus);
                continue;
            }
            if (key.Key == ConsoleKey.H)
            {
                SendMessageWithHttpStream(bus);
                continue;
            }
            return;
        }
    }

    static void SendMessageWithFileStream(IBus bus)
    {
        #region send-message-with-file-stream

        var message = new MessageWithStream
        {
            SomeProperty = "This message contains a stream",
            StreamProperty = File.OpenRead("FileToSend.txt")
        };
        bus.Send("Samples.PipelineStream.Receiver", message);

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with file stream sent");
    }

    static void SendMessageWithHttpStream(IBus bus)
    {
        #region send-message-with-http-stream

        using (var webClient = new WebClient())
        {
            var message = new MessageWithStream
            {
                SomeProperty = "This message contains a stream",
                StreamProperty = webClient.OpenRead("http://www.particular.net")
            };
            bus.Send("Samples.PipelineStream.Receiver", message);
        }

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with http stream sent");
    }

}