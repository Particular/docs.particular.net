using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PipelineHandlerTimer");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error"); 

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            await Run(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessage(endpointInstance);
                continue;
            }
            return;
        }
    }

    static async Task SendMessage(IEndpointInstance endpointInstance)
    {
        Message message = new Message();
        await endpointInstance.SendLocal(message);

        Console.WriteLine();
        Console.WriteLine("Message sent");
    }

}