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
            IBusSession busSession = endpoint.CreateBusSession();
            await Run(busSession);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static async Task Run(IBusSession busSession)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessage(busSession);
                continue;
            }
            return;
        }
    }

    static async Task SendMessage(IBusSession busSession)
    {
        Message message = new Message();
        await busSession.SendLocal(message);

        Console.WriteLine();
        Console.WriteLine("Message sent");
    }

}