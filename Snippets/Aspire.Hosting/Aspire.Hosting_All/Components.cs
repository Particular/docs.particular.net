using Aspire.Hosting;

public class Components
{
    public void Explicit(DistributedApplicationBuilder builder)
    {
        #region aspire-components-explicit

        var platform = builder.AddParticularPlatform("particular");
        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");

        var error = platform.AddServiceControlErrorInstance("servicecontrol", serviceControlDb);
        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring");
        var audit = platform.AddServiceControlAuditInstance("audit", error, serviceControlDb);
        platform.AddServicePulse("servicepulse", error, monitoring);

        #endregion
    }

    public void Error(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-components-error

        var error = platform.AddServiceControlErrorInstance("servicecontrol", serviceControlDb)
            .WithErrorQueueName("error")
            .WithThroughputQueue("particular.throughput");

        #endregion
    }

    public void Audit(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", serviceControlDb);

        #region aspire-components-audit

        var audit = platform.AddServiceControlAuditInstance("audit", error, serviceControlDb)
            .WithAuditQueueName("audit");

        #endregion
    }

    public void Monitoring(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", serviceControlDb);

        #region aspire-components-monitoring

        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring")
            .WithMonitoringQueueName("Particular.Monitoring")
            .WithThroughputQueueFrom(error);

        #endregion
    }

    public void ServicePulse(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", serviceControlDb);
        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring");

        #region aspire-components-servicepulse

        platform.AddServicePulse("servicepulse", error, monitoring);

        #endregion
    }

    public void VersionOtherThanLatest(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-components-version

        platform.AddServiceControlErrorInstance("error", serviceControlDb)
            .WithImage("particular/servicecontrol", "6.0.0");

        platform.AddServiceControlMonitoringInstance("monitoring")
            .WithImage("particular/servicecontrol-monitoring", "5.0.0"); // mismatched
        #endregion
    }
}
