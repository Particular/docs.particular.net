using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AzureTable.Table.Server";

        #region AzureTableConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.AzureTable.Table.Server");

        var useStorageTable = true;
        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();

        var connection = useStorageTable ? "UseDevelopmentStorage=true" :
            "TableEndpoint=https://localhost:8081/;AccountName=AzureTableSamples;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        var account = CloudStorageAccount.Parse(connection);
        var cloudTableClient = account.CreateCloudTableClient();
        persistence.UseCloudTableClient(cloudTableClient);
        persistence.DefaultTable("OrderSagaData");

        #endregion

        persistence.Compatibility().DisableSecondaryKeyLookupForSagasCorrelatedByProperties();

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        #region BehaviorRegistration

        endpointConfiguration.Pipeline.Register(new BehaviorProvidingDynamicTable(), "Provides a non-default table for sagas started by ship order message");

        #endregion

        var cloudTable = cloudTableClient.GetTableReference("ShipOrderSagaData");
        await cloudTable.CreateIfNotExistsAsync();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}