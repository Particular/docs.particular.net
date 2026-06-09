using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Particular.Aspire.Hosting.ServicePlatform.ThroughputReporting;
using Projects;

public class QuickStart
{
    public void AppHost(string[] args)
    {
        {
            #region aspire-quick-start-platform
        
            var builder = DistributedApplication.CreateBuilder(args);

            var platform = builder
                .AddParticularPlatform("particular")
                .AddDefaultComponents();

            builder.Build().Run();                    
            #endregion

            #region aspire-quick-start-endpoint
            
            builder.AddProject<Projects.MyEndpoint>("my-endpoint")
                   .WithParticularPlatform(platform);
            #endregion
        }

        #region aspire-quick-start-explicit
        {
            var builder = DistributedApplication.CreateBuilder(args);

            //ASB connection string
            var transport = builder.AddConnectionString("transport", "AzureServiceBus_ConnectionString");

            //platform setup with ASB transport and license file
            var platform = builder
                .AddParticularPlatform("particular")
                .WithTransportAzureServiceBus(transport)
                .WithLicenseFromFile("license.xml");

            //persistence setup
            var persistence = platform.AddPersistenceRavenDb("particular-persistence");

            //error instance setup
            var servicecontrol = platform.AddServiceControlErrorInstance("particular-error", persistence)
                .WithErrorQueueName("error")
                .WithThroughputQueue("particular.throughput");

            //audit instance setup
            platform.AddServiceControlAuditInstance("particular-audit", servicecontrol, persistence)
                .WithAuditQueueName("audit");

            //monitoring instance setup
            var monitoring = platform.AddServiceControlMonitoringInstance("particular-monitoring")
                .WithMonitoringQueueName("Particular.Monitoring")
                .WithThroughputQueueFrom(servicecontrol);

            //servicepulse instance setup
            platform.AddServicePulse("particular-servicepulse", servicecontrol, monitoring);

            builder.Build().Run();
        }
        #endregion

    }
}