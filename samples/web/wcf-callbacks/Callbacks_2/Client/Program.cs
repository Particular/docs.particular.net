using System;
using System.ServiceModel;
using System.Threading.Tasks;

static class Program
{
    static string serverUrl = "http://localhost:8080";

    static async Task Main()
    {
        Console.Title = "Samples.WcfCallbacks.Client";
        Console.WriteLine("Press 'E' to send a message that will respond with an enum");
        Console.WriteLine("Press 'I' to send a message that will respond with an int");
        Console.WriteLine("Press 'O' to send a message that will respond with an object");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.E:
                    await SendEnum()
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.I:
                    await SendInt()
                        .ConfigureAwait(false);
                    continue;
                case ConsoleKey.O:
                    await SendObject()
                        .ConfigureAwait(false);
                    continue;
            }
            return;
        }
    }

    static async Task SendEnum()
    {
        var message = new EnumMessage
        {
            Property = "The Property Value"
        };
        var response = await Send<EnumMessage, Status>(message)
            .ConfigureAwait(false);
        Console.WriteLine($"Response: {response}");
    }

    static async Task SendInt()
    {
        var message = new IntMessage
        {
            Property = "The Property Value"
        };

        var response = await Send<IntMessage, int>(message)
            .ConfigureAwait(false);
        Console.WriteLine($"Response: {response}");
    }

    #region Send

    static async Task SendObject()
    {
        var message = new ObjectMessage
        {
            Property = "The Property Value"
        };
        var response = await Send<ObjectMessage, ReplyMessage>(message)
            .ConfigureAwait(false);
        Console.WriteLine($"Response: {response.Property}");
    }

    #endregion

    #region SendHelper

    static async Task<TResponse> Send<TRequest, TResponse>(TRequest request)
    {
        using (var channelFactory = ClientChannelBuilder.GetChannelFactory<TRequest, TResponse>(serverUrl))
        using (var client = channelFactory.CreateChannel())
        {
            return await client.SendRequest(request)
                .ConfigureAwait(false);
        }
    }

    #endregion
}