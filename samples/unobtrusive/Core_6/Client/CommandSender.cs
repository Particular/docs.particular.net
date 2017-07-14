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
        Console.WriteLine("Press 'D' to send a large message that is marked to be sent using Data Bus");
        Console.WriteLine("Press 'X' to send a message that is marked with expiration time.");
        Console.WriteLine("Press any other key to exit");

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
    static Task Expiration(IEndpointInstance endpointInstance)
    {
        var messageThatExpires = new MessageThatExpires
        {
            RequestId = Guid.NewGuid()
        };
        Console.WriteLine("message with expiration was sent");
        return endpointInstance.Send("Samples.Unobtrusive.Server", messageThatExpires);
    }

    static Task Data(IEndpointInstance endpointInstance)
    {
        var requestId = Guid.NewGuid();

        var largeMessage = new LargeMessage
        {
            RequestId = requestId,
            LargeDataBus = new byte[1024*1024*5]
        };
        Console.WriteLine($"Request sent id: {requestId}");
        return endpointInstance.Send("Samples.Unobtrusive.Server", largeMessage);
    }

    static Task SendRequest(IEndpointInstance endpointInstance)
    {
        var requestId = Guid.NewGuid();

        var request = new Request
        {
            RequestId = requestId
        };
        Console.WriteLine($"Request sent id: {requestId}");
        return endpointInstance.Send("Samples.Unobtrusive.Server", request);
    }

    static Task SendCommand(IEndpointInstance endpointInstance)
    {
        var commandId = Guid.NewGuid();

        var myCommand = new MyCommand
        {
            CommandId = commandId,
            EncryptedString = "Some sensitive information"
        };
        Console.WriteLine($"Command sent id: {commandId}");
        return endpointInstance.Send("Samples.Unobtrusive.Server", myCommand);
    }


}