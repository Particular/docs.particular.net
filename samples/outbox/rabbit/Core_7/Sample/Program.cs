using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RabbitMQ.Outbox";

        #region ConfigureTransport
        var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.Outbox");
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology();
        transport.ConnectionString("host=localhost;username=rabbitmq;password=rabbitmq");
        #endregion

        #region ConfigurePersistence
        var connectionString = @"Data Source=localhost,11433;Initial Catalog=NsbRabbitMqOutbox;User=sa;Password=NServiceBus!;TrustServerCertificate=true";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
        #endregion

        endpointConfiguration.EnableInstallers();

        #region SampleSteps

        // STEP 1: Run code as is, duplicates can be observed in console and database

        // STEP 2: Uncomment this line to enable the Outbox. Duplicates will be suppressed.
        //endpointConfiguration.EnableOutbox();

        // STEP 3: Comment out this line to allow concurrent processing. Concurrency exceptions will
        // occur in the console window, but only 5 entries will be made in the database.
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        #endregion

        Helper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

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
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
