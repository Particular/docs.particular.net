using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Raven.Client.Documents;
using Raven.Client.Exceptions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;


Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);

#region Config

var endpointConfiguration = new EndpointConfiguration("Samples.RavenDB.Server");
using var documentStore = new DocumentStore
{
    Urls = ["http://localhost:8080"],
    Database = "RavenSimpleSample",
};

documentStore.Initialize();

var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
persistence.SetDefaultDocumentStore(documentStore);

#endregion

var outbox = endpointConfiguration.EnableOutbox();
outbox.SetTimeToKeepDeduplicationData(TimeSpan.FromMinutes(5));

var transport = new LearningTransport
{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(transport);

await EnsureDatabaseExists(documentStore);

Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();

static async Task EnsureDatabaseExists(DocumentStore documentStore)
{
    // create the database
    try
    {
        await documentStore.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database)));
    }
    catch (ConcurrencyException)
    {
        // intentionally ignored
    }
}
