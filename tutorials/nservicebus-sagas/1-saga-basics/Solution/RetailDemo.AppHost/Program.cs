var builder = DistributedApplication.CreateBuilder(args);


var transport = builder.AddRabbitMQ("transport");

builder.AddProject<Projects.ClientUI>("clientui")
    .WithReference(transport)
    .WaitFor(transport);

builder.AddProject<Projects.Billing>("billing")
    .WithReference(transport)
    .WaitFor(transport);

builder.AddProject<Projects.Sales>("sales")
    .WithReference(transport)
    .WaitFor(transport);

builder.AddProject<Projects.Shipping>("shipping")
    .WithReference(transport)
    .WaitFor(transport);

builder.Build().Run();
