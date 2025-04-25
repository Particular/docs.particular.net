using System;

using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);

#region AzureTableConfig

var endpointConfiguration = new EndpointConfiguration("Samples.AzureTable.Table.Server");

var useStorageTable = true;
var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();

var connection = useStorageTable ? "UseDevelopmentStorage=true" :
    "TableEndpoint=https://localhost:8081/;AccountName=AzureTableSamples;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

var tableServiceClient = new TableServiceClient(connection);
persistence.UseTableServiceClient(tableServiceClient);
persistence.DefaultTable("OrderSagaData");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();

#region BehaviorRegistration

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<BehaviorProvidingDynamicTable>>();
endpointConfiguration.Pipeline.Register(new BehaviorProvidingDynamicTable(logger), "Provides a non-default table for sagas started by ship order message");

#endregion

var tableClient = tableServiceClient.GetTableClient("ShipOrderSagaData");
await tableClient.CreateIfNotExistsAsync();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
