using Commands;
using Messages;

public class CommandSender
{
    public static async Task Start(IMessageSession messageSession)
    {
        Console.WriteLine("Press 'C' to send a command");
        Console.WriteLine("Press 'R' to send a request");
        Console.WriteLine("Press 'D' to send a large message that is marked to be sent using data bus");
        Console.WriteLine("Press 'X' to send a message that is marked with expiration time.");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.C:
                    await SendCommand(messageSession);
                    continue;
                case ConsoleKey.R:
                    await SendRequest(messageSession);
                    continue;
                case ConsoleKey.D:
                    await Data(messageSession);
                    continue;
                case ConsoleKey.X:
                    await Expiration(messageSession);
                    continue;
            }
            return;

        }
    }

    // Shut down server before sending this message, after 30 seconds, the message will be moved to Transactional dead-letter messages queue.
    static Task Expiration(IMessageSession messageSession)
    {
        var messageThatExpires = new MessageThatExpires
        {
            RequestId = Guid.NewGuid()
        };
        Console.WriteLine("message with expiration was sent");
        return messageSession.Send("Samples.Unobtrusive.Server", messageThatExpires);
    }

    static Task Data(IMessageSession messageSession)
    {
        var requestId = Guid.NewGuid();

        var largeMessage = new LargeMessage
        {
            RequestId = requestId,
            LargeClaimCheck = new byte[1024 * 1024 * 5]
        };
        Console.WriteLine($"Request sent id: {requestId}");
        return messageSession.Send("Samples.Unobtrusive.Server", largeMessage);
    }

    static Task SendRequest(IMessageSession messageSession)
    {
        var requestId = Guid.NewGuid();

        var request = new Request
        {
            RequestId = requestId
        };
        Console.WriteLine($"Request sent id: {requestId}");
        return messageSession.Send("Samples.Unobtrusive.Server", request);
    }

    static Task SendCommand(IMessageSession messageSession)
    {
        var commandId = Guid.NewGuid();

        var myCommand = new MyCommand
        {
            CommandId = commandId,
        };
        Console.WriteLine($"Command sent id: {commandId}");
        return messageSession.Send("Samples.Unobtrusive.Server", myCommand);
    }
}