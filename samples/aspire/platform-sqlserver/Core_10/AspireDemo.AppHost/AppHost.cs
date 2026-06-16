using Particular.Aspire.Hosting.ServicePlatform.ThroughputReporting;

var builder = DistributedApplication.CreateBuilder(args);

#region transport
var transport = builder.AddConnectionString("transport");
#endregion

#region platform-config
var platform = builder
    .AddParticularPlatform("particular")
    .WithTransportSqlServer(transport);
#endregion

#region persistence
var persistence = platform.AddPersistenceRavenDb("particular-persistence");
#endregion

var servicecontrol = platform.AddServiceControlErrorInstance("particular-error", persistence);

#region default-components
platform.AddDefaultComponents();
#endregion

#region endpoints
var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithParticularPlatform(platform);
#endregion

builder.Build().Run();