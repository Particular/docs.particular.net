using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Particular.Aspire.Hosting.ServicePlatform.ThroughputReporting;

public class ThroughputReporting
{
    public void AzureServiceBus(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-throughput-asb

        var tenantId = builder.AddParameter("asb-tenant-id");
        var subscriptionId = builder.AddParameter("asb-subscription-id");
        var clientId = builder.AddParameter("asb-client-id");
        var clientSecret = builder.AddParameter("asb-client-secret", secret: true);

        platform.AddServiceControlErrorInstance("servicecontrol", persistence)
            .WithThroughputReporting(new ThroughputReportingAzureServiceBus(
                tenantId: ReferenceExpression.Create($"{tenantId.Resource}"),
                subscriptionId: ReferenceExpression.Create($"{subscriptionId.Resource}"),
                clientId: ReferenceExpression.Create($"{clientId.Resource}"),
                clientSecret: ReferenceExpression.Create($"{clientSecret.Resource}")));

        #endregion
    }

    public void RabbitMQ(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-throughput-rabbitmq

        var apiUrl = builder.AddParameter("rabbit-api-url");
        var username = builder.AddParameter("rabbit-username");
        var password = builder.AddParameter("rabbit-password", secret: true);

        platform.AddServiceControlErrorInstance("servicecontrol", persistence)
            .WithThroughputReporting(new ThroughputReportingRabbitMq(
                ReferenceExpression.Create($"{apiUrl.Resource}"),
                ReferenceExpression.Create($"{username.Resource}"),
                ReferenceExpression.Create($"{password.Resource}")));

        #endregion
    }
}
