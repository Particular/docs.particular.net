var builder = DistributedApplication.CreateBuilder(args);

#region platform-config
var platform = builder
    .AddParticularPlatform("particular")
    .AddDefaultComponents();
#endregion

#region endpoints
var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithParticularPlatform(platform);
#endregion

builder.Build().Run();