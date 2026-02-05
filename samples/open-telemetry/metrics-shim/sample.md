---
title: Use a shim to expose NServiceBus.Metrics via OpenTelemetry
summary: How to set up a shim to expose NServiceBus.Metrics via OpenTelemetry
reviewed: 2024-08-22
component: Core
related:
  - nservicebus/operations/opentelemetry
---

The Particular Service Platform has built-in support for [metrics](/monitoring/metrics), which are captured by the NServiceBus.Metrics package. These metrics can be exposed to an observability backend using OpenTelemetry.
In versions of NServiceBus prior to 9.1, there was no native OpenTelemetry-support for the following [metrics](/monitoring/metrics/definitions.md):
- Critical time
- Processing time
- Handler time
- Retries

> [!NOTE]
> Starting from version 9.1, NServiceBus natively exposes these metrics via OpenTelemetry.

It is possible to expose these metrics via OpenTelemetry by using a shim.

This sample shows how to set up a shim that exposes NServiceBus.Metrics in an OpenTelemetry format, and exports them to App Insights.

## Prerequisites

This sample requires an App Insights connection string.

> [!NOTE]
> Although the sample uses Azure Application Insights, the solution itself does not require an Azure message transport. This example uses the [Learning Transport](/transports/learning/) but could be modified to run on any [transport](/transports/).

## Running the sample

1. Create an Application Insights resource in Azure
2. Copy the connection string from the Azure portal dashboard into the sample
3. Start the sample endpoint
4. Press any key to send a message, or <kbd>ESC</kbd> to quit

### Reviewing meters

Navigate to _Monitoring_ → _Metrics_ on the Azure portal dashboard for the configured Application Insight instance to start creating graphs.

> [!NOTE]
> It may take a few minutes for the meter data to populate to Azure. Meters will only appear on the dashboard once they have reported at least one value.

## Code walk-through

partial: enableotel

To enable metrics:

snippet: enable-meters

### The shim

The shim is responsible for converting NServiceBus.Metrics to OpenTelemetry metrics.

snippet: metrics-shim

> [!NOTE]
> The shim passes `QueueName` as a custom dimension which allows filtering the graphs in Application Insights. Multi-dimensional metrics are not enabled by default. Check [the Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/get-metric#enable-multi-dimensional-metrics) for instructions on how to enable this feature.

