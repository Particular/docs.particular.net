using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Callbacks.Sender";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Callbacks.Sender");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

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
            .Register<Status>(status =>
            {
                Console.WriteLine("Callback received with status:" + status);
            });

        #endregion

        Console.WriteLine("Message sent");
    }

    static void SendIntMessage(IBus bus)
    {
        #region SendIntMessage

        var message = new IntMessage();
        bus.Send("Samples.Callbacks.Receiver", message)
            .Register<int>(response =>
            {
                Console.WriteLine("Callback received with response:" + response);
            });

        #endregion

        Console.WriteLine("Message sent");
    }

    static void SendObjectMessage(IBus bus)
    {
        #region SendObjectMessage

        var message = new ObjectMessage();
        bus.Send("Samples.Callbacks.Receiver", message)
            .Register(ar =>
            {
                var localResult = (CompletionResult)ar.AsyncState;
                var response = (ObjectResponseMessage)localResult.Messages[0];
                Console.WriteLine("Callback received with response property value:" + response.Property);
            }, null);

        #endregion

        Console.WriteLine("Message sent");
    }

}