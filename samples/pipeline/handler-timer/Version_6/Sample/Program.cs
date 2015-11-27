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
            IBusContext busContext = endpoint.CreateBusContext();
            await Run(busContext);
        }
        finally
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
    }

    static async Task Run(IBusContext busContext)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessage(busContext);
                continue;
            }
            return;
        }
    }

    static async Task SendMessage(IBusContext busContext)
    {
        Message message = new Message();
        await busContext.SendLocal(message);

        Console.WriteLine();
        Console.WriteLine("Message sent");
    }

}