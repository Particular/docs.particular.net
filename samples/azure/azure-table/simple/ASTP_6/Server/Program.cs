using System;

using Azure.Data.Tables;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);

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


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
