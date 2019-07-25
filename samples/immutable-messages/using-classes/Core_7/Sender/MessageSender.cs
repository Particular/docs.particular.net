using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

class MessageSender
{

    public static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'M' to send a message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.M:
                    await SendMessage(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
            }
            return;
        }
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        var data = Guid.NewGuid().ToString();

        Console.WriteLine($"Message sent, data: {data}");
        var myMessage = new MyMessage(data);
        return endpointInstance.Send(myMessage);
    }
}