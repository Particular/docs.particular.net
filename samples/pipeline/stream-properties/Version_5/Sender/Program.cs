using System;
using System.IO;
using System.Net;
using NServiceBus;

class Program
{
    static IStartableBus bus;

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.PipelineStream.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        #region configure-stream-storage
        busConfiguration.SetStreamStorageLocation("..\\..\\..\\storage");
        #endregion
        busConfiguration.EnableInstallers();
        bus = Bus.Create(busConfiguration);
        bus.Start();
        Run();
    }


    static void Run()
    {
        Console.WriteLine("Press 'F' to send a message with a file stream");
        Console.WriteLine("Press 'H' to send a message with a http stream");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.F)
            {
                SendMessageWithFileStream();
                continue;
            }
            if (key.Key == ConsoleKey.H)
            {
                SendMessageWithHttpStream();
            }
        }
    }

    static void SendMessageWithFileStream()
    {
        #region send-message-with-file-stream

        MessageWithStream message = new MessageWithStream
        {
            SomeProperty = "This message contains a stream",
            StreamProperty = File.OpenRead("FileToSend.txt")
        };
        bus.Send("Sample.PipelineStream.Receiver", message);
        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with file stream sent");
    }
    static void SendMessageWithHttpStream()
    {
        #region send-message-with-http-stream

        using (WebClient webClient = new WebClient())
        {
            MessageWithStream message = new MessageWithStream
            {
                SomeProperty = "This message contains a stream",
                StreamProperty = webClient.OpenRead("http://www.particular.net")
            };
            bus.Send("Sample.PipelineStream.Receiver", message);
        }
        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with http stream sent");
    }
}

