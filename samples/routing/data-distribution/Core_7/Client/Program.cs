using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DataDistribution.Client."+Utils.GetUniqueDataDistributionId();

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        const string endpointName = "Samples.DataDistribution.Client";

        var mainConfig = MainConfig(endpointName);

        var distributionConfig = DistributionConfig(endpointName);

        var mainEndpoint = await Endpoint.Start(mainConfig)
            .ConfigureAwait(false);
        var distributionEndpoint = await Endpoint.Start(distributionConfig)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
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
        await mainEndpoint.Stop()
            .ConfigureAwait(false);
        await distributionEndpoint.Stop()
            .ConfigureAwait(false);
    }

    static EndpointConfiguration DistributionConfig(string endpointName)
    {
        #region DistributionEndpointName
        var distributionEndpointName = $"{endpointName}.{Utils.GetUniqueDataDistributionId()}";
        var distributionConfig = new EndpointConfiguration(distributionEndpointName);

        #endregion

        #region DistributionEndpointTypes
        var typesToExclude = AllTypes
            .Where(t => t.Namespace != "DataDistribution")
            .ToArray();
        var scanner = distributionConfig.AssemblyScanner();
        scanner.ExcludeTypes(typesToExclude);
        #endregion

        var transport = distributionConfig.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(
            publisherEndpoint: "Samples.DataDistribution.Server",
            eventType: typeof(OrderAccepted));

        ApplyDefaults(distributionConfig);
        return distributionConfig;
    }

    static EndpointConfiguration MainConfig(string endpointName)
    {
        #region MainConfig

        var mainConfig = new EndpointConfiguration(endpointName);

        var typesToExclude = AllTypes
            .Where(t => t.Namespace == "DataDistribution")
            .ToArray();
        var scanner = mainConfig.AssemblyScanner();
        scanner.ExcludeTypes(typesToExclude);
        var transport = mainConfig.UseTransport<MsmqTransport>();
        var mainRouting = transport.Routing();
        mainRouting.RouteToEndpoint(
            messageType: typeof(PlaceOrder),
            destination: "Samples.DataDistribution.Server");
        mainRouting.RegisterPublisher(
            publisherEndpoint: "Samples.DataDistribution.Server",
            eventType: typeof(OrderAccepted));

        #endregion

        ApplyDefaults(mainConfig);
        return mainConfig;
    }

    static Type[] AllTypes => typeof(Program).Assembly.GetTypes();

    static void ApplyDefaults(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
    }

}