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

        var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic), "host=localhost;username=rabbitmq;password=rabbitmq")
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(rabbitMqTransport);
        endpointConfiguration.EnableInstallers();
        var connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");
        Helper.EnsureDatabaseExists(connectionString);

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
        endpointConfiguration.Recoverability().Immediate(c => c.OnMessageBeingRetried((r, _) =>
        {
            Console.WriteLine($"Retry {r.RetryAttempt} for sequence number {r.Headers["sequence-number"]}");
            return Task.CompletedTask;
        }));

        //endpointConfiguration.EnableOutbox();
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var numberOfConcurrentMessages = 10;
        var numberOfDuplicateMessages = 1;

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