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
        Console.Title = "Samples.DataDistribution.Client.2";

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        const string endpointName = "Samples.DataDistribution.Client";
        var mainConfig = FilterNamespace1(endpointName);

        var distributionConfig = FilterNamespace2(endpointName);

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
                await mainEndpoint.Send(message)
                    .ConfigureAwait(false);
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

    static EndpointConfiguration FilterNamespace2(string endpointName)
    {
        var distributionConfig = new EndpointConfiguration($"{endpointName}.{GetUniqueDataDistributionId()}");

        #region FilterNamespace2

        var typesToExclude = AllTypes
            .Where(t => t.Namespace != "DataDistribution")
            .ToArray();
        distributionConfig.ExcludeTypes(typesToExclude);

        #endregion

        var transport = distributionConfig.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(
            publisherEndpoint: "Samples.DataDistribution.Server",
            eventType: typeof(OrderAccepted));
        ApplyDefaults(distributionConfig);
        return distributionConfig;
    }

    static EndpointConfiguration FilterNamespace1(string endpointName)
    {
        var mainConfig = new EndpointConfiguration(endpointName);
        var typesToExclude = AllTypes.Where(t => t.Namespace == "DataDistribution").ToArray();
        mainConfig.ExcludeTypes(typesToExclude);

        #region FilterNamespace1

        var transport = mainConfig.UseTransport<MsmqTransport>();
        var mainRouting = transport.Routing();

        #endregion

        mainRouting.RouteToEndpoint(typeof(PlaceOrder), "Samples.DataDistribution.Server");
        mainRouting.RegisterPublisher(typeof(OrderAccepted), "Samples.DataDistribution.Server");
        ApplyDefaults(mainConfig);
        return mainConfig;
    }

    static Type[] AllTypes => typeof(Program).Assembly.GetTypes();

    static void ApplyDefaults(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
    }

    static string GetUniqueDataDistributionId()
    {
        return "2";
    }
}