---
title: Monitoring NServiceBus endpoints with Application Insights
summary: How to configure NServiceBus to export OpenTelemetry traces and meters to Application Insights
reviewed: 2022-07-15
component: Core
related:
- nservicebus/operations/opentelemetry
---

## Introduction

[Azure Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) (App Insights) provides monitoring and alerting capabilities that can be leveraged to monitor the health of NServiceBus endpoints.

This sample shows how to capture NServiceBus OpenTelemetry traces and export them to App Insights. The sample simulates message load and includes a 10% failure rate on processing messages.

## Prerequisites

This sample requires an App Insights connection string.

Note: Although the sample uses Azure Application Insights, the solution itself does not have to run on an Azure message transport. This example uses the [Learning Transport](/transports/learning/) but could be modified to run on any [transport](/transports/).

## Running the sample

1. Create an Application Insights resource in Azure
2. Copy the connection string from the Azure portal dashboard into the sample
3. Start the sample endpoint
4. Press any key to send a message, or ESC to quit

### Reviewing traces

1. On the Aure portal dashboard, open the _Investigate_ → _Performance_ panel
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

![Graph tracking success and failed metrics in Application Insights](metrics-dashboard.png)

NOTE: It may take a few minutes for the meter data to populate to Azure. Meters will only appear on the dashboard once they have reported at least one value.

## Code walk-through

The OpenTelemetry instrumentation is enabled on the endpoint.

snippet: enable-open-telemetry

### Tracing

The endpoint configures an OpenTelemetry trace provider that includes the `NServiceBus.Core` source and exports traces to Azure Monitor.

snippet: enable-tracing

### Meters

The endpoint also configures an OpenTelemetry meter provider that includes the `NServiceBus.Core` meter and exports data to Azure Monitor.

snippet: enable-meters

#### Meter shim

NOTE: The Azure Monitor exporter package does not currently support exporting meter data. The sample includes custom code to collect the meter.

The custom meter exporter captures all meters that begin with `nservicebus.` of the `long` sum type, and forwards them to a `TelemetryClient`.

snippet: custom-meter-exporter

This exporter is installed into the meter provider builder with a custom extension method. The exporter is wrapped by a `PeriodicExportingMetricReader` instance that executes the exporter once every 10 seconds.

snippet: custom-meter-exporter-installation

NOTE: The shim passes `QueueName` as a custom dimension which allows filtering the graphs in Application Insights. Multi-dimensional metrics are not enabled by default. Check [the Microsoft Documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/get-metric#enable-multi-dimensional-metrics) for instructions on how to enable this feature.