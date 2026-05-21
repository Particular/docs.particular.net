using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.Sender");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.MakeInstanceUniquelyAddressable("1");
        endpointConfiguration.EnableCallbacks();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await host.StartAsync();

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
                await SendEnumMessage(messageSession);
                continue;
            }
            if (key.Key == ConsoleKey.I)
            {
                await SendIntMessage(messageSession);
                continue;
            }
            if (key.Key == ConsoleKey.O)
            {
                await SendObjectMessage(messageSession);
                continue;
            }
            break;
        }
        await host.StopAsync();
    }


    static async Task SendEnumMessage(IMessageSession messageSession)
    {
        Console.WriteLine("Message sent");

        #region SendEnumMessage

        var message = new EnumMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var status = await messageSession.Request<Status>(message, sendOptions);
        Console.WriteLine($"Callback received with status:{status}");

        #endregion
    }

    static async Task SendIntMessage(IMessageSession messageSession)
    {
        Console.WriteLine("Message sent");

        #region SendIntMessage

        var message = new IntMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var response = await messageSession.Request<int>(message, sendOptions);
        Console.WriteLine($"Callback received with response:{response}");

        #endregion
    }

    static async Task SendObjectMessage(IMessageSession messageSession)
    {
        Console.WriteLine("Message sent");

        #region SendObjectMessage

        var message = new ObjectMessage();
        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Callbacks.Receiver");
        var response = await messageSession.Request<ObjectResponseMessage>(message, sendOptions);
        Console.WriteLine($"Callback received with response property value:{response.Property}");

        #endregion
    }
}
