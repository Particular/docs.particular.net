using System;

using Azure.Data.Tables;

using NServiceBus;

Console.Title = "Samples.AzureTable.Simple.Server";

#region AzureTableConfig

var useStorageTable = true;
var endpointConfiguration = new EndpointConfiguration("Samples.AzureTable.Simple.Server");

var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();

var connection = useStorageTable ? "UseDevelopmentStorage=true" :
    "TableEndpoint=https://localhost:8081/;AccountName=AzureTableSamples;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

var tableServiceClient = new TableServiceClient(connection);
persistence.UseTableServiceClient(tableServiceClient);

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
