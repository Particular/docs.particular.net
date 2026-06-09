using Aspire.Hosting;
using Particular.Aspire.Hosting.ServicePlatform.Platform;

public class Production
{
    public void HostPorts(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");
        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence);
        var monitoring = platform.AddServiceControlMonitoringInstance("monitoring");

        #region aspire-host-ports

        platform.AddServicePulse("servicepulse", error, monitoring)
            .WithEndpoint("http", e =>
            {
                e.Port = 9091;
            });

        #endregion
    }

    public void ImagePinning(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-image-pinning

        var serviceControlVersion = "x.y.z";

        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence)
            .WithImageTag(serviceControlVersion);

        platform.AddServiceControlAuditInstance("audit", error, persistence)
            .WithImageTag(serviceControlVersion);

        platform.AddServiceControlMonitoringInstance("monitoring")
            .WithImageTag(serviceControlVersion);

        #endregion
    }

    public void RunMode(DistributedApplicationBuilder builder)
    {
        var platform = builder.AddParticularPlatform("particular");
        var persistence = platform.AddPersistenceRavenDb("ravendb");

        #region aspire-run-mode

        var error = platform.AddServiceControlErrorInstance("servicecontrol", persistence)
            .WithRunMode(PlatformRunMode.Run);

        platform.AddServiceControlAuditInstance("audit", error, persistence)
            .WithRunMode(PlatformRunMode.Run);

        platform.AddServiceControlMonitoringInstance("monitoring")
            .WithRunMode(PlatformRunMode.Run);

        #endregion
    }
}
