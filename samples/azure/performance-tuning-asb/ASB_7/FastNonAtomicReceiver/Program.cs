using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    public static ReceiveCounter ReceiveCounter = new ReceiveCounter();
    static ILog log = LogManager.GetLogger<Program>();

    static async Task Main()
    {
        Console.Title = "Samples.ASB.Performance.FastNonAtomicReceiver";

        ReceiveCounter.Subscribe(i => log.Warn($"Process {i} messages per second"));

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Receiver");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        var topology = transport.UseForwardingTopology();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region fast-non-atomic-receiver-config

        transport.Transactions(TransportTransactionMode.ReceiveOnly);

        var queues = transport.Queues();
        queues.EnablePartitioning(true);

        var numberOfCores = Environment.ProcessorCount;

        // concurrency allowed
        var perReceiverConcurrency = 128;

        // increase number of receivers as much as bandwidth allows
        var numberOfReceivers = 32;

        var globalConcurrency = numberOfReceivers * perReceiverConcurrency;

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(globalConcurrency);

        // as is prefetching
        var receivers = transport.MessageReceivers();
        receivers.PrefetchCount(perReceiverConcurrency);

        var factories = transport.MessagingFactories();
        factories.NumberOfMessagingFactoriesPerNamespace(numberOfReceivers);
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
}