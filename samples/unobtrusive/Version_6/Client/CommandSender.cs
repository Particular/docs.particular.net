using System;
using System.Threading.Tasks;
using Commands;
using Messages;
using NServiceBus;

public class CommandSender
{

    public static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'C' to send a command");
        Console.WriteLine("Press 'R' to send a request");
        Console.WriteLine("Press 'E' to send a message that is marked as Express");
        Console.WriteLine("Press 'D' to send a large message that is marked to be sent using Data Bus");
        Console.WriteLine("Press 'X' to send a message that is marked with expiration time.");
        Console.WriteLine("Press any key to exit");


        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.C:
                    await SendCommand(endpointInstance);
                    continue;
                case ConsoleKey.R:
                    await SendRequest(endpointInstance);
                    continue;
                case ConsoleKey.E:
                    await Express(endpointInstance);
                    continue;
                case ConsoleKey.D:
                    await Data(endpointInstance);
                    continue;
                case ConsoleKey.X:
                    await Expiration(endpointInstance);
                    continue;
            }
            return;

        }
    }


    // Shut down server before sending this message, after 30 seconds, the message will be moved to Transactional dead-letter messages queue.
    static async Task Expiration(IEndpointInstance endpointInstance)
    {
        await endpointInstance.Send(new MessageThatExpires
                 {
                     RequestId = new Guid()
                 });
        Console.WriteLine("message with expiration was sent");
    }

    static async Task Data(IEndpointInstance endpointInstance)
    {
        Guid requestId = Guid.NewGuid();

        await endpointInstance.Send(new LargeMessage
        {
            RequestId = requestId,
            LargeDataBus = new byte[1024*1024*5]
        });

        Console.WriteLine("Request sent id: " + requestId);
    }

    static async Task Express(IEndpointInstance endpointInstance)
    {
        Guid requestId = Guid.NewGuid();

        await endpointInstance.Send(new RequestExpress
        {
            RequestId = requestId
        });

        Console.WriteLine("Request sent id: " + requestId);
    }

    static async Task SendRequest(IEndpointInstance endpointInstance)
    {
        Guid requestId = Guid.NewGuid();

        await endpointInstance.Send(new Request
        {
            RequestId = requestId
        });

        Console.WriteLine("Request sent id: " + requestId);
    }

    static async Task SendCommand(IEndpointInstance endpointInstance)
    {
        Guid commandId = Guid.NewGuid();

        await endpointInstance.Send(new MyCommand
                 {
                     CommandId = commandId,
                     EncryptedString = "Some sensitive information"
                 });

        Console.WriteLine("Command sent id: " + commandId);
    }


}