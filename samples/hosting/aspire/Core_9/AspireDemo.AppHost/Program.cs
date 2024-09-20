#region app-host
var builder = DistributedApplication.CreateBuilder(args);

var transport = builder.AddRabbitMQ("transport")
    .WithHealthCheck();

builder.AddProject<Projects.Billing>("billing")
    .WithReference(transport)
    .WaitFor(transport);

var sales = builder.AddProject<Projects.Sales>("sales")
    .WithReference(transport)
    .WaitFor(transport);

builder.AddProject<Projects.ClientUI>("clientui")
    .WithReference(transport)
    .WaitFor(transport)
    .WaitFor(sales);

var database = builder.AddPostgres("database")
    // NOTE: This is needed as the call to AddDatabase below
    // does not actually create the database
    .WithEnvironment("POSTGRES_DB", "shipping-db")
    .WithPgWeb();

var shippingDb = database.AddDatabase("shipping-db")
    .WithHealthCheck();

builder.AddProject<Projects.Shipping>("shipping")
    .WithReference(transport)
    .WaitFor(transport)
    .WithReference(shippingDb)
    .WaitFor(shippingDb);

builder.Build().Run();
#endregion