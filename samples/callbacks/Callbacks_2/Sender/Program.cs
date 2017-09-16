using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.Callbacks.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.Sender");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.MakeInstanceUniquelyAddressable("1");
        endpointConfiguration.EnableCallbacks();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
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
                await SendEnumMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            if (key.Key == ConsoleKey.I)
            {
                await SendIntMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            if (key.Key == ConsoleKey.O)
            {
                await SendObjectMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            break;
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }


    static async Task SendEnumMessage(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Message sent");

        #region SendEnumMessage

        var message = new EnumMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var status = await endpointInstance.Request<Status>(message, sendOptions)
            .ConfigureAwait(false);
        Console.WriteLine($"Callback received with status:{status}");

        #endregion
    }

    static async Task SendIntMessage(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Message sent");

        #region SendIntMessage

        var message = new IntMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var response = await endpointInstance.Request<int>(message, sendOptions)
            .ConfigureAwait(false);
        Console.WriteLine($"Callback received with response:{response}");

        #endregion
    }

    static async Task SendObjectMessage(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Message sent");

        #region SendObjectMessage

        var message = new ObjectMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var response = await endpointInstance.Request<ObjectResponseMessage>(message, sendOptions)
            .ConfigureAwait(false);
        Console.WriteLine($"Callback received with response property value:{response.Property}");

        #endregion
    }
}