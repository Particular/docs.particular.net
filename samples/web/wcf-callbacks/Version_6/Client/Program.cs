using System;
using System.ServiceModel;
using System.Threading.Tasks;

static class Program
{
    static string serverUrl = "http://localhost:8080";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.WriteLine("Press 'E' to send a message that will respond with an enum");
        Console.WriteLine("Press 'I' to send a message that will respond with an int");
        Console.WriteLine("Press 'O' to send a message that will respond with an object");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.E:
                    await SendEnum();
                    continue;
                case ConsoleKey.I:
                    await SendInt();
                    continue;
                case ConsoleKey.O:
                    await SendObject();
                    continue;
            }
            return;
        }
    }

    static async Task SendEnum()
    {
        EnumMessage message = new EnumMessage
        {
            Property = "The Property Value"
        };
        Status response = await Send<EnumMessage, Status>(message);
        Console.WriteLine("Response: " + response);
    }

    static async Task SendInt()
    {
        IntMessage message = new IntMessage
        {
            Property = "The Property Value"
        };

        int response = await Send<IntMessage, int>(message);
        Console.WriteLine("Response: " + response);
    }

    #region Send
    static async Task SendObject()
    {
        ObjectMessage message = new ObjectMessage
        {
            Property = "The Property Value"
        };
        ReplyMessage response = await Send<ObjectMessage, ReplyMessage>(message);
        Console.WriteLine("Response: " + response.Property);
    }
    #endregion

    #region SendHelper
    static async Task<TResponse> Send<TRequest,TResponse>(TRequest request)
    {
        using (ChannelFactory<ICallbackService<TRequest, TResponse>> channelFactory = ClientChannelBuilder.GetChannelFactory<TRequest, TResponse>(serverUrl))
        using (ICallbackService<TRequest, TResponse> client = channelFactory.CreateChannel())
        {
            return await client.SendRequest(request);
        }
    }
#endregion
}