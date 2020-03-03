using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static ILog log = LogManager.GetLogger<Program>();
    public static ReceiveCounter ReceiveCounter = new ReceiveCounter();

    static async Task Main()
    {
        Console.Title = "Samples.ASB.Performance.FastAtomicSenderReceiver";

        ReceiveCounter.Subscribe(i => log.Warn($"Processed {i} & sent {i * SomeMessageHandler.NumberOfMessagesToSend} messages"));

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Receiver");
        var transportConfiguration = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transportConfiguration.ConnectionString(connectionString);

        var topology = transportConfiguration.UseForwardingTopology();

        var destinationName = "Samples.ASB.Performance.Destination";
        await EnsureDestinationQueueExists(destinationName, connectionString)
            .ConfigureAwait(false);
        var routing = transportConfiguration.Routing();
        routing.RouteToEndpoint(typeof(SomeMessage), destinationName);

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region fast-atomic-send-receive-config

        transportConfiguration.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var queues = transportConfiguration.Queues();
        queues.EnablePartitioning(true);

        // values 2 and 4 work best, as tx is serializable it makes no sense to allow many concurrent tasks
        var perReceiverConcurrency = 8;

        // increase number of receivers as much as bandwidth allows (probably less than receiver due to send volume)
        var numberOfReceivers = 16;

        var globalConcurrency = numberOfReceivers * perReceiverConcurrency;

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(globalConcurrency);
        var receivers = transportConfiguration.MessageReceivers();
        receivers.PrefetchCount(20);

        var factories = transportConfiguration.MessagingFactories();
        factories.NumberOfMessagingFactoriesPerNamespace(numberOfReceivers * 2);
        transportConfiguration.NumberOfClientsPerEntity(numberOfReceivers);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Receiver is ready to receive messages");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task EnsureDestinationQueueExists(string receiverPath, string connectionString)
    {
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
        if (!await namespaceManager.QueueExistsAsync(receiverPath)
            .ConfigureAwait(false))
        {
            var queueDescription = new QueueDescription(receiverPath)
            {
                EnablePartitioning = true,
                EnableBatchedOperations = true
            };
            await namespaceManager.CreateQueueAsync(queueDescription)
                .ConfigureAwait(false);
        }
    }
}