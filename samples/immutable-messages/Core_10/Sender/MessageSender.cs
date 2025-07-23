using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class MessageSender(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Press 'C' to send a message (using a class with a non-default constructor");
        Console.WriteLine("Press 'I' to send a message (using a private class that implements a shared interface");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.C:
                    await SendMessageAsClass(messageSession);
                    continue;
                case ConsoleKey.I:
                    await SendMessageAsInterface(messageSession);
                    continue;
            }
            return;
        }

    }


    static Task SendMessageAsClass(IMessageSession messageSession)
    {
        var data = Guid.NewGuid().ToString();

        Console.WriteLine($"Message sent, data: {data}");
        var myMessage = new UsingClasses.Messages.MyMessage(data);
        return messageSession.Send(myMessage);
    }

    static Task SendMessageAsInterface(IMessageSession messageSession)
    {
        var data = Guid.NewGuid().ToString();

        Console.WriteLine($"Message sent, data: {data}");
        #region immutable-messages-as-interface-sending
        var myMessage = new Messages.MyMessageImpl()
        {
            Data = data
        };
        return messageSession.Send(myMessage);
        #endregion
    }
}