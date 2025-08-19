using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "RabbitMQOutbox";

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
        var connectionString = @"Server=localhost,11433;Initial Catalog=NsbRabbitMqOutbox;User Id=SA;Password=NServiceBus!;Max Pool Size=100;Encrypt=false";

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
        #endregion

        endpointConfiguration.EnableInstallers();

        #region SampleSteps

        // STEP 1: Run code as is, duplicates can be observed in console and database

        // STEP 2: Uncomment this line to enable the Outbox. Duplicates will be suppressed.
        // endpointConfiguration.EnableOutbox();

        // STEP 3: Comment out this line to allow concurrent processing. Concurrency exceptions will
        // occur in the console window, but only 5 entries will be made in the database.
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        #endregion

        Helper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Endpoint started. Press Enter to send 5 sets of duplicate messages...");
        Console.ReadLine();

        for (var i = 0; i < 5; i++)
        {
            var myMessage = new MyMessage();
            await Helper.SendDuplicates(endpointInstance, myMessage, totalCount: 2);
        }

        await Task.Delay(5000);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
