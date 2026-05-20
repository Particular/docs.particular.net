#region app-host

using Particular.Aspire.Hosting.ServicePlatform.Transport;

var builder = DistributedApplication.CreateBuilder(args);

var transportUserName = builder.AddParameter("transportUserName", "guest", secret: true);
var transportPassword = builder.AddParameter("transportPassword", "guest", secret: true);

var transport = builder.AddRabbitMQ("transport", transportUserName, transportPassword)
    .WithManagementPlugin(15672)
    .WithUrlForEndpoint("management", url => url.DisplayText = "RabbitMQ Management");

transportUserName.WithParentRelationship(transport);
transportPassword.WithParentRelationship(transport);

var database = builder.AddPostgres("database");

database.WithPgAdmin(resource =>
{
    resource.WithParentRelationship(database);
    resource.WithUrlForEndpoint("http", url => url.DisplayText = "pgAdmin");
});

var shippingDB = database.AddDatabase("shipping-db");

var platform = builder
    .AddParticularPlatform("particular")
    .WithTransportRabbitMq(RabbitMqRouting.QuorumConventionalRouting, transport)
    .AddDefaultComponents();

builder.AddProject<Projects.Billing>("Billing")
    .WithParticularPlatform(platform);

var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithParticularPlatform(platform);

builder.AddProject<Projects.Shipping>("Shipping")
    .WithReference(shippingDB)
    .WaitFor(shippingDB)
    .WithParticularPlatform(platform);

builder.Build().Run();
#endregion