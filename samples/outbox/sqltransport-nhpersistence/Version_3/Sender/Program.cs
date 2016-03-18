using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SQLNHibernateOutbox.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SQLNHibernateOutbox.Sender");
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region SenderConfiguration

        endpointConfiguration
            .UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True");

        endpointConfiguration
            .UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True");
        
        endpointConfiguration.EnableOutbox();

        #endregion

        endpointConfiguration.DisableFeature<FirstLevelRetries>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press enter to publish a message");
            Console.WriteLine("Press any key to exit");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                await endpoint.Publish(new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}