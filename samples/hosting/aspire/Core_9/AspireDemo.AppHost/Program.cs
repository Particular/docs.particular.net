#region app-host
var builder = DistributedApplication.CreateBuilder(args);

var transport = builder.AddRabbitMQ("transport")
    .WithManagementPlugin(15672)
    .WithUrlForEndpoint("management", url => url.DisplayText = "RabbitMQ Management");

var database = builder.AddPostgres("database");

database.WithPgAdmin(resource =>
{
    resource.WithParentRelationship(database);
    resource.WithUrlForEndpoint("http", url => url.DisplayText = "pgAdmin");
});

var shippingDb = database.AddDatabase("shipping-db");

var ravenDB = builder.AddContainer("ServiceControl-RavenDB", "particular/servicecontrol-ravendb")
    .WithHttpEndpoint(8080, 8080)
    .WithUrlForEndpoint("http", url => url.DisplayText = "Management Studio");

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

var servicePulse = builder.AddContainer("ServicePulse", "particular/servicepulse")
    .WithEnvironment("ENABLE_REVERSE_PROXY", "false")
    .WithHttpEndpoint(9090, 9090)
    .WithUrlForEndpoint("http", url => url.DisplayText = "ServicePulse")
    .WaitFor(serviceControl)
    .WaitFor(audit)
    .WaitFor(monitoring);

builder.AddProject<Projects.Billing>("Billing")
    .WithReference(transport)
    .WaitFor(servicePulse);

var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithReference(transport)
    .WaitFor(servicePulse);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WithReference(transport)
    .WaitFor(sales)
    .WaitFor(servicePulse);

builder.AddProject<Projects.Shipping>("Shipping")
    .WithReference(transport)
    .WithReference(shippingDb)
    .WaitFor(shippingDb)
    .WaitFor(servicePulse);

builder.Build().Run();
#endregion