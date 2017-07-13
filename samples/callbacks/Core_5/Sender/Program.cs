using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Callbacks.Sender";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Callbacks.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'E' to send a message with an enum return");
            Console.WriteLine("Press 'I' to send a message with an int return");
            Console.WriteLine("Press 'O' to send a message with an object return");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
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


    static void SendEnumMessage(IBus bus)
    {
        #region SendEnumMessage

        var message = new EnumMessage();
        bus.Send("Samples.Callbacks.Receiver", message)
            .Register<Status>(
                callback: status =>
                {
                    Console.WriteLine($"Callback received with status:{status}");
                });

        #endregion

        Console.WriteLine("Message sent");
    }

    static void SendIntMessage(IBus bus)
    {
        #region SendIntMessage

        var message = new IntMessage();
        bus.Send("Samples.Callbacks.Receiver", message)
            .Register<int>(
                callback: response =>
                {
                    Console.WriteLine($"Callback received with response:{response}");
                });

        #endregion

        Console.WriteLine("Message sent");
    }

    static void SendObjectMessage(IBus bus)
    {
        #region SendObjectMessage

        var message = new ObjectMessage();
        bus.Send("Samples.Callbacks.Receiver", message)
            .Register(
                callback: asyncResult =>
                {
                    var localResult = (CompletionResult) asyncResult.AsyncState;
                    var response = (ObjectResponseMessage) localResult.Messages[0];
                    Console.WriteLine($"Callback received with response property value:{response.Property}");
                },
                state: null);

        #endregion

        Console.WriteLine("Message sent");
    }
}