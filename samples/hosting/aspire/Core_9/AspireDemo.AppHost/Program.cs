#region app-host
var builder = DistributedApplication.CreateBuilder(args);

var transport = builder.AddRabbitMQ("transport")
    .WithManagementPlugin(15672);

builder.AddProject<Projects.Billing>("Billing")
    .WithReference(transport)
    .WaitFor(transport);

var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithReference(transport)
    .WaitFor(transport);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WithReference(transport)
    .WaitFor(transport)
    .WaitFor(sales);

var database = builder.AddPostgres("database")
    // NOTE: This is needed as the call to AddDatabase below
    // does not actually create the database
    .WithEnvironment("POSTGRES_DB", "shipping-db")
    .WithPgWeb();

var shippingDb = database.AddDatabase("shipping-db");

builder.AddProject<Projects.Shipping>("Shipping")
    .WithReference(transport)
    .WaitFor(transport)
    .WithReference(shippingDb)
    .WaitFor(shippingDb);

var ravenDB = builder.AddContainer("ServiceControl-RavenDB", "particular/servicecontrol-ravendb")
    .WithHttpEndpoint(8080, 8080);

var audit = builder.AddContainer("ServiceControl-Audit", "particular/servicecontrol-audit")
    .WithEnvironment("TRANSPORTTYPE", "RabbitMQ.QuorumConventionalRouting")
    .WithEnvironment("CONNECTIONSTRING", transport)
    .WithEnvironment("RAVENDB_CONNECTIONSTRING", ravenDB.GetEndpoint("http"))
    .WithArgs("--setup-and-run")
    .WithHttpEndpoint(44444, 44444)
    .WithHttpHealthCheck("api/configuration")
    .WaitFor(transport)
    .WaitFor(ravenDB);

var serviceControl = builder.AddContainer("ServiceControl", "particular/servicecontrol")
    .WithEnvironment("TRANSPORTTYPE", "RabbitMQ.QuorumConventionalRouting")
    .WithEnvironment("CONNECTIONSTRING", transport)
    .WithEnvironment("RAVENDB_CONNECTIONSTRING", ravenDB.GetEndpoint("http"))
    //.WithEnvironment("REMOTEINSTANCES", $"[{{\"api_uri\":\"{audit.GetEndpoint("http")}\"}}]") //Aspire bug prevents this from working
    .WithEnvironment("REMOTEINSTANCES", () =>
    {
        var endpoint = audit.GetEndpoint("http");
        return $"[{{\"api_uri\":\"http://{endpoint.Resource.Name}:{endpoint.Port}\"}}]";
    })
    .WithArgs("--setup-and-run")
    .WithHttpEndpoint(33333, 33333)
    .WithHttpHealthCheck("api/configuration")
    .WaitFor(transport)
    .WaitFor(ravenDB);

var monitoring = builder.AddContainer("ServiceControl-Monitoring", "particular/servicecontrol-monitoring")
    .WithEnvironment("TRANSPORTTYPE", "RabbitMQ.QuorumConventionalRouting")
    .WithEnvironment("CONNECTIONSTRING", transport)
    .WithArgs("--setup-and-run")
    .WithHttpEndpoint(33633, 33633)
    .WithHttpHealthCheck("connection")
    .WaitFor(transport);

builder.AddContainer("ServicePulse", "particular/servicepulse")
    .WithEnvironment("ENABLE_REVERSE_PROXY", "false")
    .WithHttpEndpoint(9090, 9090)
    .WaitFor(serviceControl)
    .WaitFor(audit)
    .WaitFor(monitoring);

builder.Build().Run();
#endregion