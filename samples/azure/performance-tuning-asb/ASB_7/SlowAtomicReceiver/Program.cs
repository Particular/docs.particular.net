using System;
using System.Threading.Tasks;
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
        Console.Title = "Samples.ASB.Performance.SlowAtomicReceiver";

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

        #region slow-atomic-receiver-config

        var queues = transport.Queues();
        queues.EnablePartitioning(true);

        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        var receivers = transport.MessageReceivers();
        receivers.PrefetchCount(0);

        var factories = transport.MessagingFactories();
        factories.NumberOfMessagingFactoriesPerNamespace(1);
        transport.NumberOfClientsPerEntity(1);

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