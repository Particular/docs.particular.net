using System;
using System.IO;
using System.Net;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PipelineStream.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #region configure-stream-storage

        busConfiguration.SetStreamStorageLocation("..\\..\\..\\storage");

        #endregion

        busConfiguration.EnableInstallers();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
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
            ConsoleKeyInfo key = Console.ReadKey();

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

        MessageWithStream message = new MessageWithStream
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

        using (WebClient webClient = new WebClient())
        {
            MessageWithStream message = new MessageWithStream
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