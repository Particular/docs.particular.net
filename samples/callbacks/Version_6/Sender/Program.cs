using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Callbacks.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'E' to send a message with an enum return");
            Console.WriteLine("Press 'I' to send a message with an int return");
            Console.WriteLine("Press 'O' to send a message with an object return");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.E)
                {
                    SendEnumMessage(bus);
                    continue;
                }
                if (key.Key == ConsoleKey.I)
                {
                    SendIntMessage(bus);
                    continue;
                }
                if (key.Key == ConsoleKey.O)
                {
                    SendObjectMessage(bus);
                    continue;
                }
                return;
            }
        }
    }


    static async void SendEnumMessage(IBus bus)
    {
        #region SendEnumMessage

        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        EnumMessage message = new EnumMessage();
        Status status = await bus.Request<Status>(message, sendOptions);
        Console.WriteLine("Callback received with status:" + status);

        #endregion

        Console.WriteLine("Message sent");
    }

    static async void SendIntMessage(IBus bus)
    {
        #region SendIntMessage

        IntMessage message = new IntMessage();

        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        int response = await bus.Request<int>(message, sendOptions);
        Console.WriteLine("Callback received with response:" + response);

        #endregion

        Console.WriteLine("Message sent");
    }

    static async void SendObjectMessage(IBus bus)
    {
        #region SendObjectMessage

        ObjectMessage message = new ObjectMessage();
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");

        ObjectResponseMessage response = await bus.Request<ObjectResponseMessage>(message, sendOptions);
        Console.WriteLine("Callback received with response property value:" + response.Property);
        #endregion

        Console.WriteLine("Message sent");
    }
}