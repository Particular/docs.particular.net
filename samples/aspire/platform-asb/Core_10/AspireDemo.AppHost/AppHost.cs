#region app-host

using Particular.Aspire.Hosting.ServicePlatform.Transport;

var builder = DistributedApplication.CreateBuilder(args);

var transport = builder.AddConnectionString("transport", "AzureServiceBus_ConnectionString");

var platform = builder
    .AddParticularPlatform("particular")
    .WithTransportAzureServiceBus(transport)
    .AddDefaultComponents();

var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithParticularPlatform(platform);

builder.Build().Run();
#endregion