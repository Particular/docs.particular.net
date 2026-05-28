using Particular.Aspire.Hosting.ServicePlatform.ThroughputReporting;

var builder = DistributedApplication.CreateBuilder(args);

#region transport
var transport = builder.AddConnectionString("transport", "AzureServiceBus_ConnectionString");
#endregion

#region platform-config
var platform = builder
    .AddParticularPlatform("particular")
    .WithTransportAzureServiceBus(transport);
#endregion

#region persistence
var persistence = platform.AddPersistenceRavenDb("particular-persistence");
#endregion

#region throughput-reporting-params
var asbTenantId = builder.AddParameter("asb-tenant-id", "[YOUR_VALUE]");
var asbSubscriptionId = builder.AddParameter("asb-subscription-id", "[YOUR_VALUE]");
var asbClientId = builder.AddParameter("asb-client-id", "[YOUR_VALUE]");
var asbClientSecret = builder.AddParameter("asb-client-secret", "[YOUR_VALUE]", secret: true);
#endregion

var servicecontrol = platform.AddServiceControlErrorInstance("particular-error", persistence)
    .WithThroughputReporting(new ThroughputReportingAzureServiceBus(
        tenantId: ReferenceExpression.Create($"{asbTenantId.Resource}"),
        subscriptionId: ReferenceExpression.Create($"{asbSubscriptionId.Resource}"),
        clientId: ReferenceExpression.Create($"{asbClientId.Resource}"),
        clientSecret: ReferenceExpression.Create($"{asbClientSecret.Resource}")));

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