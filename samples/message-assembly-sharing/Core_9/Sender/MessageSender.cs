namespace Sender;

using Shared;

class MessageSender
{
    public static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'C' to send a message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.C:
                    await SendMessage(endpointInstance);
                    continue;
            }
            return;
        }
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        var data = Guid.NewGuid().ToString();

        Console.WriteLine($"Message sent, data: {data}");
        var myCommand = new MyCommand { Data = data };
        return endpointInstance.Send(myCommand);
    }
}