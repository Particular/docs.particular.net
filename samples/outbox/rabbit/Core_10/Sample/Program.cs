using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "RabbitMQOutbox";

        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Error);

        var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.Outbox");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.EnableInstallers();
        var connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");
        Helper.EnsureDatabaseExists(connectionString);

        endpointConfiguration.Recoverability().Immediate(c => c.OnMessageBeingRetried((r, _) =>
        {
            r.Headers.TryGetValue("sequence-number",  out var sequenceNumber);
            Console.WriteLine($"Retry {r.RetryAttempt} {r.Headers[NServiceBus.Headers.EnclosedMessageTypes]} for sequence number {sequenceNumber}");
            return Task.CompletedTask;
        }));
        var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic), "host=localhost;username=rabbitmq;password=rabbitmq")
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        endpointConfiguration.UseTransport(rabbitMqTransport);
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));

        endpointConfiguration.EnableOutbox();
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var numberOfConcurrentMessages = 5;
        var numberOfDuplicateMessages = 2;

        var correlationId = Guid.NewGuid().ToString();
        for (var i = 0; i < numberOfConcurrentMessages; i++)
        {
            var messageId = Guid.NewGuid().ToString();
            for (var j = 0; j < numberOfDuplicateMessages; j++)
            {
                var sendOptions = new SendOptions();

                sendOptions.RouteToThisEndpoint();
                sendOptions.SetHeader("sequence-number", i.ToString());
                sendOptions.SetMessageId(messageId);

                var myMessage = new MyMessage
                {
                    CorrelationID = correlationId,
                    SequenceNumber = i
                };

                await endpointInstance.Send(myMessage, sendOptions);
            }
        }

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}