using Aspire.Hosting;

public class Components
{
    public void Explicit(DistributedApplicationBuilder builder)
    {
        #region aspire-components-explicit

        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");

        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence);
        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring");
        var audit = platform.AddServiceControlAuditInstance("audit", error, persistence);
        platform.AddServicePulse("servicepulse", error, monitoring);

        #endregion
    }

    public void Error(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-components-error

        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence)
            .WithErrorQueueName("error")
            .WithThroughputQueue("particular.throughput");

        #endregion
    }

    public void Audit(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence);

        #region aspire-components-audit

        var audit = platform.AddServiceControlAuditInstance("audit", error, persistence)
            .WithAuditQueueName("audit");

        #endregion
    }

    public void Monitoring(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence);

        #region aspire-components-monitoring

        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring")
            .WithMonitoringQueueName("Particular.Monitoring")
            .WithThroughputQueueFrom(error);

        #endregion
    }

    public void ServicePulse(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence);
        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring");

        #region aspire-components-servicepulse

        platform.AddServicePulse("servicepulse", error, monitoring);

        #endregion
    }
}
