using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Azure.Cosmos;
public static class Helper
{
    public static async Task CreateContainerAndDbIfNotExists(string connectionString)
    {
        var cosmosClient = new CosmosClient(connectionString);
        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("Samples.Database.Demo");
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: "Outbox",
            partitionKeyPath: "/messageId"
        );
    }

    public static Task SendDuplicates<TMessage>(IMessageSession context, TMessage message, int totalCount)
    {
        var duplicatedMessageId = Guid.NewGuid().ToString();

        var tasks = Enumerable.Range(0, totalCount)
            .Select(i =>
            {
                var options = new SendOptions();
                options.SetHeader("PartitionKeyHeader", "/messageId");//set the partition key
                options.RouteToThisEndpoint();
                options.SetMessageId(duplicatedMessageId);

                return context.Send(message, options);
            });

        return Task.WhenAll(tasks);
    }
}