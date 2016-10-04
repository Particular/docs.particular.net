using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
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
        Console.Title = "Samples.ASB.Performance.FastNonAtomicSenderReceiver";

        ReceiveCounter.Subscribe(i => _logger.Warn("Processed " + i + " & sent " + i * SomeMessageHandler.NumberOfMessagesToSend + " messages"));

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Receiver");
        var transportConfiguration = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transportConfiguration.ConnectionString(connectionString);
        var topology = transportConfiguration.UseTopology<ForwardingTopology>();

        var destinationName = "Samples.ASB.Performance.Destination";
        await EnsureDestinationQueueExists(destinationName, connectionString).ConfigureAwait(false);
        transportConfiguration.Routing().RouteToEndpoint(typeof(SomeMessage), destinationName);

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region fast-non-atomic-config

        transportConfiguration.Transactions(TransportTransactionMode.ReceiveOnly);

        transportConfiguration.Queues().EnablePartitioning(true);

        var perReceiverConcurrency = 8; //values between 2 and 8 work best, as tx is serializable it makes no sense to allow many concurrent tasks
        var numberOfReceivers = 16; // increase number of receivers as much as bandwidth allows (probably less than receiver due to send volume)
        var globalConcurrency = numberOfReceivers * perReceiverConcurrency;

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(globalConcurrency);
        transportConfiguration.MessageReceivers().PrefetchCount(0);

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

    static async Task EnsureDestinationQueueExists(string receiverPath, string connectionString)
    {
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
        if (!await namespaceManager.QueueExistsAsync(receiverPath).ConfigureAwait(false))
        {
            await namespaceManager.CreateQueueAsync(new QueueDescription(receiverPath)
            {
                EnablePartitioning = true,
                EnableBatchedOperations = true
            });
        }
    }

}