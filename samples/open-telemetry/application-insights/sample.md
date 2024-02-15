---
title: Monitoring NServiceBus endpoints with Application Insights
summary: How to configure NServiceBus to export OpenTelemetry traces and meters to Application Insights
reviewed: 2024-01-17
component: Core
related:
  - nservicebus/operations/opentelemetry
redirects:
  - samples/logging/application-insights
  - samples/tracing
---

[Azure Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) (App Insights) provides monitoring and alerting capabilities that can be leveraged to monitor the health of NServiceBus endpoints.

This sample shows how to capture NServiceBus OpenTelemetry traces and export them to App Insights. The sample simulates message load as well as a 10% failure rate on processing messages.

## Prerequisites

This sample requires an App Insights connection string.

Note: Although the sample uses Azure Application Insights, the solution itself does not require an Azure message transport. This example uses the [Learning Transport](/transports/learning/) but could be modified to run on any [transport](/transports/).

## Running the sample

1. Create an Application Insights resource in Azure
2. Copy the connection string from the Azure portal dashboard into the sample
3. Start the sample endpoint
4. Press any key to send a message, or <kbd>ESC</kbd> to quit

### Reviewing traces

1. On the Azure portal dashboard, open the _Investigate_ → _Performance_ panel
2. Drill into the samples
3. Review the custom properties

![Timeline view of a trace in Application Insights](trace-timeline.png)

### Reviewing meters

1. Open the Azure portal dashboard for the configured Application Insight instance
2. Navigate to _Monitoring_ → _Metrics_
3. Add metrics with the following information:
- Metric Namespace: `azure.applicationinsights`
- Metrics:
  - `nservicebus.messaging.successes`
  - `nservicebus.messaging.failures`
  - `nservicebus.messaging.fetches`
  - `nservicebus.messaging.processingtime`
  - `nservicebus.messaging.criticaltime`

![Graph tracking success and failed metrics in Application Insights](metrics-dashboard.png)

NOTE: It may take a few minutes for the meter data to populate to Azure. Meters will only appear on the dashboard once they have reported at least one value.

## Code walk-through

The OpenTelemetry instrumentation is enabled on the endpoint.

snippet: enable-open-telemetry

### Tracing

The endpoint configures an OpenTelemetry trace provider that includes the `NServiceBus.Core` source and export traces to Azure Monitor.

snippet: enable-tracing

### Meters

The endpoint also configures an OpenTelemetry meter provider that includes the `NServiceBus.Core` meter and export data to Azure Monitor.

snippet: enable-meters

#### Critical time and processing time

[Critical time and processing time captured by the metrics package](/monitoring/metrics/definitions.md#metrics-captured) is not yet included in the built-in open telemetry support so a shim is needed to make sure that the endpoint exposes them as open telemetry metrics.

snippet: metrics-shim
