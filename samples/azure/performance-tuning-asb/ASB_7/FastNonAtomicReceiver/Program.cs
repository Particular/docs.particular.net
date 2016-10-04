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

        var perReceiverConcurrency = 128; // concurrency allowed
        var numberOfReceivers = 32; // increase number of receivers as much as bandwidth allows
        var globalConcurrency = numberOfReceivers * perReceiverConcurrency;

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(globalConcurrency);
        transportConfiguration.MessageReceivers().PrefetchCount(perReceiverConcurrency); // as is prefetching

        transportConfiguration.MessagingFactories().NumberOfMessagingFactoriesPerNamespace(numberOfReceivers);
        transportConfiguration.NumberOfClientsPerEntity(numberOfReceivers);

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