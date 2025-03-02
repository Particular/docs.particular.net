using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.SqlServer;
using Sender;

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             services.AddHostedService<InputLoopService>();
             Console.Title = "Sender";
         }).UseNServiceBus(x =>
         {

             var endpointConfiguration = new EndpointConfiguration("Samples.SqlOutbox.Sender");
             endpointConfiguration.EnableInstallers();
             endpointConfiguration.SendFailedMessagesTo("error");

             #region SenderConfiguration

             // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlOutbox;Integrated Security=True;Max Pool Size=100;Encrypt=false
             var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlOutbox;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

             var transport = new SqlServerTransport(connectionString)
             {
                 DefaultSchema = "sender",
                 TransportTransactionMode = TransportTransactionMode.ReceiveOnly
             };
             transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
             transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

             endpointConfiguration.UseTransport(transport);

             var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
             persistence.ConnectionBuilder(
                 connectionBuilder: () =>
                 {
                     return new SqlConnection(connectionString);
                 });
             var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
             dialect.Schema("sender");
             persistence.TablePrefix("");

             transport.Subscriptions.DisableCaching = true;
             transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName(
                 table: "Subscriptions",
                 schema: "dbo");

             endpointConfiguration.EnableOutbox();

             endpointConfiguration.UseSerialization<SystemJsonSerializer>();

             #endregion

             SqlHelper.CreateSchema(connectionString, "sender");
             Console.WriteLine("Press enter to send a message");
             Console.WriteLine("Press any key to exit");
             return endpointConfiguration;
         });
}