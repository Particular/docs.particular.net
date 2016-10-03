using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    public static ReceiveCounter ReceiveCounter = new ReceiveCounter();
    private static ILog _logger = LogManager.GetLogger<Program>();

    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Performance.FastNonAtomicReceiver";

        ReceiveCounter.Subscribe(i => _logger.Warn("Process " + i + " messages per second"));

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Receiver");
        var transportConfiguration = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transportConfiguration.ConnectionString(connectionString);
        var topology = transportConfiguration.UseTopology<ForwardingTopology>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region fast-non-atomic-config

        transportConfiguration.Transactions(TransportTransactionMode.ReceiveOnly);

        transportConfiguration.Queues().EnablePartitioning(true);

        var numberOfCores = Environment.ProcessorCount;
        var concurrency = numberOfCores * 32; //256 on test machine with 8 logical cores

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(concurrency);
        transportConfiguration.MessageReceivers().PrefetchCount(concurrency*2);

        transportConfiguration.MessagingFactories().NumberOfMessagingFactoriesPerNamespace(32);
        transportConfiguration.NumberOfClientsPerEntity(32);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Receiver is ready to receive messages");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

}