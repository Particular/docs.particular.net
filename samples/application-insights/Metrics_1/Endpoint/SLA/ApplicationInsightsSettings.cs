using System;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

public class ApplicationInsightsSettings
{
    internal const string EndpointSLAKey = "EndpointSLA";
    internal const string EndpointSLACeilingKey = "EndpointSLACeiling";
    EndpointConfiguration endpointConfiguration;

    internal ApplicationInsightsSettings(EndpointConfiguration endpointConfiguration)
    {
        this.endpointConfiguration = endpointConfiguration;
    }

    /// <summary>
    /// Enables the Time To Breach Service Level Agreement (SLA) metric.
    /// </summary>
    /// <param name="sla">The SLA to use. Must be greater than <see cref="TimeSpan.Zero" />.</param>
    /// <param name="ceiling">The metric reports the time until the violation will occur. Specify the ceiling to round any values down to this value.</param>
    public void EnableServiceLevelAgreementViolationCountdownMetric(TimeSpan sla, TimeSpan ceiling)
    {
        var settings = endpointConfiguration.GetSettings();
        settings.Set(EndpointSLAKey, sla);
        settings.Set(EndpointSLACeilingKey, ceiling);
    }
}