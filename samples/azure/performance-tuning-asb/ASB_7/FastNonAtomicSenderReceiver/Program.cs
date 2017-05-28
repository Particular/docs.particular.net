using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    public static ReceiveCounter ReceiveCounter = new ReceiveCounter();
    static ILog log = LogManager.GetLogger<Program>();

    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Performance.FastNonAtomicSenderReceiver";

        ReceiveCounter.Subscribe(i => log.Warn($"Processed {i} & sent {i*SomeMessageHandler.NumberOfMessagesToSend} messages"));

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Receiver");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        var topology = transport.UseForwardingTopology();

        var destinationName = "Samples.ASB.Performance.Destination";
        await EnsureDestinationQueueExists(destinationName, connectionString)
            .ConfigureAwait(false);
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(SomeMessage), destinationName);

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region fast-non-atomic-sender-receiver-config

        transport.Transactions(TransportTransactionMode.ReceiveOnly);

        var queues = transport.Queues();
        queues.EnablePartitioning(true);

        //lower concurrency if sending more message per receive
        var perReceiverConcurrency = 128;

        // increase number of receivers as much as bandwidth allows (probably less than receiver due to send volume)
        var numberOfReceivers = 16;

        var globalConcurrency = numberOfReceivers*perReceiverConcurrency;

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(globalConcurrency);
        var receivers = transport.MessageReceivers();
        receivers.PrefetchCount(perReceiverConcurrency);

        var factories = transport.MessagingFactories();
        factories.NumberOfMessagingFactoriesPerNamespace(numberOfReceivers*2);
        transport.NumberOfClientsPerEntity(numberOfReceivers);

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