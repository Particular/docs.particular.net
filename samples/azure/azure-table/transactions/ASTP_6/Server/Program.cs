using System;

using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;


Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);

#region AzureTableConfig

var endpointConfiguration = new EndpointConfiguration("Samples.AzureTable.Transactions.Server");
endpointConfiguration.EnableOutbox();

var useStorageTable = true;
var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();

var connection = useStorageTable ? "UseDevelopmentStorage=true" :
    "TableEndpoint=https://localhost:8081/;AccountName=AzureTableSamples;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

var tableServiceClient = new TableServiceClient(connection);
persistence.UseTableServiceClient(tableServiceClient);

persistence.DefaultTable("Server");

#endregion


#region BehaviorRegistration

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<OrderIdHeaderAsPartitionKeyBehavior>>();
endpointConfiguration.Pipeline.Register(new OrderIdHeaderAsPartitionKeyBehavior(logger), "Extracts a partition key from a header");
endpointConfiguration.Pipeline.Register(new OrderIdAsPartitionKeyBehavior.Registration());

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var transport = endpointConfiguration.UseTransport<LearningTransport>();
transport.Transactions(TransportTransactionMode.ReceiveOnly);
endpointConfiguration.EnableInstallers();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
