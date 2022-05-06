using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();

        Console.Title = "Samples.Azure.ServiceBus.AsbEndpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.AsbEndpoint");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.AddDeserializer<XmlSerializer>();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));

        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.Azure.ServiceBus.MsmqEndpoint");
        
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to send a command");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey().Key;
            if (key != ConsoleKey.Enter)
            {
                break;
            }

            var prop = new string(Enumerable.Range(0, 3).Select(i => letters[random.Next(letters.Length)]).ToArray());
            await endpointInstance.Send(new MyCommand { Property = prop }, sendOptions).ConfigureAwait(false);
            Console.WriteLine($"\nCommand with value '{prop}' sent");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}