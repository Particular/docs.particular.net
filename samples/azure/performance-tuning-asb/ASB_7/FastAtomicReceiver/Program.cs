using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static ILog log = LogManager.GetLogger<Program>();
    public static ReceiveCounter ReceiveCounter = new ReceiveCounter();

    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Performance.FastAtomicReceiver";

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

        #region fast-atomic-receiver-config

        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var queues = transport.Queues();
        queues.EnablePartitioning(true);

        // values 2 and 4 work best, as tx is serializable it makes no sense to allow many concurrent tasks
        var perReceiverConcurrency = 2;

        // increase number of receivers as much as bandwidth allows
        var numberOfReceivers = 16;

        var globalConcurrency = numberOfReceivers*perReceiverConcurrency;

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(globalConcurrency);
        var receivers = transport.MessageReceivers();
        receivers.PrefetchCount(20);

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