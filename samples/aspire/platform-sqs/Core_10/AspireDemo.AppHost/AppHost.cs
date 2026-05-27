var builder = DistributedApplication.CreateBuilder(args);

#region platform-config
var region = Amazon.RegionEndpoint.USWest1;
string? queueNamePrefix = null;

var accessKey = builder.AddParameter("accessKey");
var accessSecret = builder.AddParameter("secretKey", secret:true);

var platform = builder
    .AddParticularPlatform("particular")
    .WithTransportAmazonSqs(
        region.SystemName,
        accessKey.Resource,
        accessSecret.Resource,
        conf =>
        {
            conf.QueueNamePrefix = queueNamePrefix;
            conf.TopicNamePrefix = queueNamePrefix;
        }
    )
    .AddDefaultComponents();
#endregion

#region endpoints
var sales = builder.AddProject<Projects.Sales>("Sales")
    .WaitFor(platform)
    .WithEnvironment("QUEUE_PREFIX", queueNamePrefix)
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithEnvironment("QUEUE_PREFIX", queueNamePrefix)
    .WithParticularPlatform(platform);
#endregion

builder.Build().Run();