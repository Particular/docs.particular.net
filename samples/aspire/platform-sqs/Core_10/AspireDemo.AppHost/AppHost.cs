var builder = DistributedApplication.CreateBuilder(args);

#region platform-config
var region = Amazon.RegionEndpoint.USWest1;

//optional: prefix to avoid conflicts with existing queues in the AWS account.
var resourceNamePrefix = "particular-aspire-demo-";

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
            conf.QueueNamePrefix = resourceNamePrefix;
            conf.TopicNamePrefix = resourceNamePrefix;
        }
    )
    .AddDefaultComponents();
#endregion

#region endpoints
var sales = builder.AddProject<Projects.Sales>("Sales")
    .WaitFor(platform)
    .WithEnvironment("RESOURCE_NAME_PREFIX", resourceNamePrefix)
    .WithParticularPlatform(platform);

builder.AddProject<Projects.ClientUI>("ClientUI")
    .WaitFor(sales)
    .WithEnvironment("RESOURCE_NAME_PREFIX", resourceNamePrefix)
    .WithParticularPlatform(platform);
#endregion

builder.Build().Run();