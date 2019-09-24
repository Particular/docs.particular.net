using System;
using System.Threading.Tasks;
using NServiceBus;

class MessageSender
{

    public static async Task Start(IEndpointInstance endpointInstance)
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
                    await SendMessageAsClass(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.I:
                    await SendMessageAsInterface(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
            }
            return;
        }
    }

    static Task SendMessageAsClass(IEndpointInstance endpointInstance)
    {
        var data = Guid.NewGuid().ToString();

        Console.WriteLine($"Message sent, data: {data}");
        var myMessage = new UsingClasses.Messages.MyMessage(data);
        return endpointInstance.Send(myMessage);
    }

    static Task SendMessageAsInterface(IEndpointInstance endpointInstance)
    {
        var data = Guid.NewGuid().ToString();

        Console.WriteLine($"Message sent, data: {data}");
#region immutable-messages-as-interface-sending
        var myMessage = new Messages.MyMessageImpl()
        {
            Data = data
        };
        return endpointInstance.Send(myMessage);
#endregion
    }
}