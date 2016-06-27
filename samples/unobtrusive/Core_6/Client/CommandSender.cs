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
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.C:
                    await SendCommand(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.R:
                    await SendRequest(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.E:
                    await Express(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.D:
                    await Data(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.X:
                    await Expiration(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
            }
            return;

        }
    }


    // Shut down server before sending this message, after 30 seconds, the message will be moved to Transactional dead-letter messages queue.
    static async Task Expiration(IEndpointInstance endpointInstance)
    {
        var messageThatExpires = new MessageThatExpires
        {
            RequestId = new Guid()
        };
        await endpointInstance.Send(messageThatExpires)
            .ConfigureAwait(false);
        Console.WriteLine("message with expiration was sent");
    }

    static async Task Data(IEndpointInstance endpointInstance)
    {
        var requestId = Guid.NewGuid();

        var largeMessage = new LargeMessage
        {
            RequestId = requestId,
            LargeDataBus = new byte[1024*1024*5]
        };
        await endpointInstance.Send(largeMessage)
            .ConfigureAwait(false);

        Console.WriteLine($"Request sent id: {requestId}");
    }

    static async Task Express(IEndpointInstance endpointInstance)
    {
        var requestId = Guid.NewGuid();

        var requestExpress = new RequestExpress
        {
            RequestId = requestId
        };
        await endpointInstance.Send(requestExpress)
            .ConfigureAwait(false);

        Console.WriteLine($"Request sent id: {requestId}");
    }

    static async Task SendRequest(IEndpointInstance endpointInstance)
    {
        var requestId = Guid.NewGuid();

        var request = new Request
        {
            RequestId = requestId
        };
        await endpointInstance.Send(request)
            .ConfigureAwait(false);

        Console.WriteLine($"Request sent id: {requestId}");
    }

    static async Task SendCommand(IEndpointInstance endpointInstance)
    {
        var commandId = Guid.NewGuid();

        var myCommand = new MyCommand
        {
            CommandId = commandId,
            EncryptedString = "Some sensitive information"
        };
        await endpointInstance.Send(myCommand)
            .ConfigureAwait(false);

        Console.WriteLine($"Command sent id: {commandId}");
    }


}