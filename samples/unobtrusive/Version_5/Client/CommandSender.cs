using System;
using Commands;
using Messages;
using NServiceBus;

public class CommandSender
{

    public static void Start(IBus bus)
    {
        Console.WriteLine("Press 'C' to send a command");
        Console.WriteLine("Press 'R' to send a request");
        Console.WriteLine("Press 'E' to send a message that is marked as Express");
        Console.WriteLine("Press 'D' to send a large message that is marked to be sent using Data Bus");
        Console.WriteLine("Press 'X' to send a message that is marked with expiration time.");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            string cmd = Console.ReadKey().Key.ToString().ToLower();
            switch (cmd)
            {
                case "c":
                    SendCommand(bus);
                    break;

                case "r":
                    SendRequest(bus);
                    break;

                case "e":
                    Express(bus);
                    break;

                case "d":
                    Data(bus);
                    break;

                case "x":
                    Expiration(bus);
                    break;
            }
        }
    }


    /// <summary>
    /// Shut down server before sending this message, after 30 seconds, the message will be moved to Transactional dead-letter messages queue.
    /// </summary>
    static void Expiration(IBus bus)
    {
        bus.Send(new MessageThatExpires
                 {
                     RequestId = new Guid()
                 });
        Console.WriteLine("message with expiration was sent");
    }

    static void Data(IBus bus)
    {
        Guid requestId = Guid.NewGuid();

        bus.Send(new LargeMessage
        {
            RequestId = requestId,
            LargeDataBus = new byte[1024*1024*5]
        });

        Console.WriteLine("Request sent id: " + requestId);
    }

    static void Express(IBus bus)
    {
        Guid requestId = Guid.NewGuid();

        bus.Send(new RequestExpress
        {
            RequestId = requestId
        });

        Console.WriteLine("Request sent id: " + requestId);
    }

    static void SendRequest(IBus bus)
    {
        Guid requestId = Guid.NewGuid();

        bus.Send(new Request
        {
            RequestId = requestId
        });

        Console.WriteLine("Request sent id: " + requestId);
    }

    static void SendCommand(IBus bus)
    {
        Guid commandId = Guid.NewGuid();

        bus.Send(new MyCommand
                 {
                     CommandId = commandId,
                     EncryptedString = "Some sensitive information"
                 })
            .Register<CommandStatus>(outcome => Console.WriteLine("Server returned status: " + outcome));

        Console.WriteLine("Command sent id: " + commandId);

    }


}