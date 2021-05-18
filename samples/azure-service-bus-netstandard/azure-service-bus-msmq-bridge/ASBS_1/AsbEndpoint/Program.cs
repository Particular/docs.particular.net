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
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.AddDeserializer<XmlSerializer>();

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);

        #region connect-asb-side-of-bridge

        var routing = transport.Routing();
        var bridge = routing.ConnectToBridge("Bridge");

        #endregion

        #region route-command-via-bridge

        bridge.RouteToEndpoint(typeof(MyCommand), "Samples.Azure.ServiceBus.MsmqEndpoint");

        #endregion

        #region subscribe-to-event-via-bridge

        bridge.RegisterPublisher(typeof(MyEvent), "Samples.Azure.ServiceBus.MsmqEndpoint");

        #endregion

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