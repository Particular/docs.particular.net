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

        #region ConfigureTransport

        var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.Outbox");

        var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic), "host=localhost;username=rabbitmq;password=rabbitmq")
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(rabbitMqTransport);

        #endregion

        #region ConfigurePersistence

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbRabbitMqOutbox;Integrated Security=True;Max Pool Size=100;Encrypt=false
        // Password must match value in docker-compose.yml
        var connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));

        #endregion

        //endpointConfiguration.EnableInstallers();

//        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);


        endpointConfiguration.Recoverability().Immediate(c => c.OnMessageBeingRetried((r, _) =>
        {
            Console.WriteLine($"Retry {r.RetryAttempt} for sequence number {r.Headers["sequence-number"]}");
            return Task.CompletedTask;
        }));


        Helper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        int count = 10;

        var correlationId = Guid.NewGuid().ToString();
        for (var i = 0; i < count; i++)
        {
            var sendOptions = new SendOptions();

            sendOptions.RouteToThisEndpoint();

            sendOptions.SetHeader("sequence-number", i.ToString());
            var myMessage = new MyMessage
            {
                CorrelationID = correlationId,
                SequenceNumber = i
            };
            await endpointInstance.Send(myMessage, sendOptions);
        }

        await Task.Delay(5000);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}