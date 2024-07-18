using System;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using OpenTelemetry.Resources;

namespace Core_9
{
    class HostIdentifier
    {
        void Align(EndpointConfiguration endpointConfiguration, IServiceCollection services)
        {
            #region opentelemetry-align-host-id
            // Generate instance ID shared by all components

            // Generate deterministic uuid v4 via
            // https://github.com/Faithlife/FaithlifeUtility/blob/master/src/Faithlife.Utility/GuidUtility.cs
            var deterministicValue = "MyEndpoint@" + Dns.GetHostName();
            Guid serviceInstanceId = GuidUtility.Create(deterministicValue); // or Guid.NewGuid()

            // OpenTelemetry
            services.AddOpenTelemetry()
                .ConfigureResource(builder =>
                    builder.AddService("MyService", serviceInstanceId: serviceInstanceId.ToString()));

            // NServiceBus
            endpointConfiguration.UniquelyIdentifyRunningInstance()
                .UsingCustomDisplayName("original-instance")
                .UsingCustomIdentifier(serviceInstanceId);

            // ServiceControl Metrics
            endpointConfiguration
                .EnableMetrics()
                // Not required when already set via UsingCustomIdentifier
                .SendMetricDataToServiceControl("particular.monitoring",
                    TimeSpan.FromMinutes(1), serviceInstanceId.ToString());
            #endregion
        }

        class GuidUtility
        {
            public static Guid Create(string deterministicValue)
            {
                throw new NotImplementedException();
            }
        }
    }
}