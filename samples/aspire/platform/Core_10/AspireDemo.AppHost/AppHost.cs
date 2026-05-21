#region app-host

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("database");

database.WithPgAdmin(resource =>
{
    resource.WithParentRelationship(database);
    resource.WithUrlForEndpoint("http", url => url.DisplayText = "pgAdmin");
});

var platform = builder
    .AddParticularPlatform("particular")
    .AddDefaultComponents();

var sales = builder.AddProject<Projects.Sales>("Sales")
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithParticularPlatform(platform);

builder.Build().Run();
#endregion