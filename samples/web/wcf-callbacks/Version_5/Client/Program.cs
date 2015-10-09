using System;
using System.ServiceModel;
using System.Threading.Tasks;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    private static async Task AsyncMain()
    {
        Console.WriteLine("Press 'E' to send a message that will respond with an enum");
        Console.WriteLine("Press 'I' to send a message that will respond with an enum");
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
        using (var channelFactory = ClientChannelBuilder.GetChannelFactory<EnumMessage, Status>("http://localhost:8080"))
        {
            ICallbackService<EnumMessage, Status> client = channelFactory.CreateChannel();
            var communicationObject = (ICommunicationObject) client;
            EnumMessage message = new EnumMessage
            {
                Property = "The Property Value"
            };
            var response = await client.SendRequest(message);
            Console.WriteLine("Response: " + response);
            communicationObject.Close();
        }
    }

    static async Task SendInt()
    {
        using (var channelFactory = ClientChannelBuilder.GetChannelFactory<IntMessage, int>("http://localhost:8080"))
        {
            ICallbackService<IntMessage, int> client = channelFactory.CreateChannel();

            var communicationObject = (ICommunicationObject) client;
            IntMessage message = new IntMessage
            {
                Property = "The Property Value"
            };
            var response = await client.SendRequest(message);
            Console.WriteLine("Response: " + response);
            communicationObject.Close();
        }
    }

    static async Task SendObject()
    {
        using (var channelFactory = ClientChannelBuilder.GetChannelFactory<ObjectMessage, ReplyMessage>("http://localhost:8080"))
        {
            ICallbackService<ObjectMessage, ReplyMessage> client = channelFactory.CreateChannel();
            var communicationObject = (ICommunicationObject) client;
            ObjectMessage message = new ObjectMessage
            {
                Property = "The Property Value"
            };
            var response = await client.SendRequest(message);
            Console.WriteLine("Response: " + response.Property);
            communicationObject.Close();
        }
    }
}