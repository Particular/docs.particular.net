using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.MsmqEndpoint";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.MsmqEndpoint");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<XmlSerializer>();
        endpointConfiguration.AddDeserializer<NewtonsoftSerializer>();
        var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();

        var bridge = routing.ConnectToBridge("Bridge");
        bridge.RouteToEndpoint(typeof(MyCommand), "Samples.Azure.ServiceBus.AsbEndpoint");

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
            await endpointInstance.Send(new MyCommand { Property = prop }).ConfigureAwait(false);
            Console.WriteLine($"\nCommand with value '{prop}' sent");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);

       
    }
}
