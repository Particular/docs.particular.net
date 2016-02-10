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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.PipelineHandlerTimer");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error"); 

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
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