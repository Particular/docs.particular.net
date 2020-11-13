using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AzureTable.Simple.Server";

        #region AzureTableConfig

        var useStorageTable = true;
        var endpointConfiguration = new EndpointConfiguration("Samples.AzureTable.Simple.Server");

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>();

        var connection = useStorageTable ? "UseDevelopmentStorage=true" :
            throw new NotSupportedException("NServiceBus.Persistence.AzureStorage doesn't support Azure Cosmos DB");

        persistence.ConnectionString(connection);

        #endregion

        persistence.AssumeSecondaryIndicesExist();

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}