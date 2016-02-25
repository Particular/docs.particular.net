using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncRun().GetAwaiter().GetResult();
    }

    static async Task AsyncRun()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        Random random = new Random();
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

        #region SenderConfiguration

        endpointConfiguration.UseTransport<SqlServerTransport>();
        endpointConfiguration.UsePersistence<NHibernatePersistence>();
        endpointConfiguration.Pipeline.Register<OutboxLoopbackReceiveBehavior.Registration>();
        endpointConfiguration.Pipeline.Register<OutboxLoopbackSendBehavior.Registration>();
        endpointConfiguration.EnableOutbox();

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press enter to publish a message");
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());

            await endpoint.Publish(new OrderSubmitted
            {
                OrderId = orderId,
                Value = random.Next(100)
            });
        }

        await endpoint.Stop();
    }
}