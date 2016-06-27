using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.DataDistribution.Client.1";

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        const string endpointName = "Samples.DataDistribution.Client";

        #region MainConfig
        var mainConfig = new EndpointConfiguration(endpointName);

        mainConfig.ExcludeTypes(AllTypes.Where(t => t.Namespace == "DataDistribution").ToArray());

        var mainRouting = mainConfig.UseTransport<MsmqTransport>().UnicastRouting();
        mainRouting.RouteToEndpoint(typeof(PlaceOrder), "Samples.DataDistribution.Server");
        mainRouting.AddPublisher("Samples.DataDistribution.Server", typeof(OrderAccepted));
        #endregion

        ApplyDefaults(mainConfig);

        #region DistributionConfig
        var distributionConfig = new EndpointConfiguration($"{endpointName}.{GetUniqueDataDistributionId()}");
        distributionConfig.ExcludeTypes(AllTypes.Where(t => t.Namespace != "DataDistribution").ToArray());
        var distributionRouting = distributionConfig.UseTransport<MsmqTransport>().UnicastRouting();
        distributionRouting.AddPublisher("Samples.DataDistribution.Server", typeof(OrderAccepted));
        #endregion

        ApplyDefaults(distributionConfig);

        var mainEndpoint = await Endpoint.Start(mainConfig)
            .ConfigureAwait(false);
        var distributionEndpoint = await Endpoint.Start(distributionConfig)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                Console.WriteLine($"Placing order {orderId}");
                var message = new PlaceOrder
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                await mainEndpoint.Send(message).ConfigureAwait(false);
            }
        }
        finally
        {
            await mainEndpoint.Stop()
                .ConfigureAwait(false);
            await distributionEndpoint.Stop()
                .ConfigureAwait(false);
        }
    }

    static Type[] AllTypes => typeof(Program).Assembly.GetTypes();

    static void ApplyDefaults(EndpointConfiguration mainConfiguration)
    {
        mainConfiguration.UseSerialization<JsonSerializer>();
        mainConfiguration.UsePersistence<InMemoryPersistence>();
        mainConfiguration.EnableInstallers();
        mainConfiguration.SendFailedMessagesTo("error");
    }

    static string GetUniqueDataDistributionId()
    {
        return "1";
    }
}