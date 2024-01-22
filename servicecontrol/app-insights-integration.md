---
title: Integrating with Azure Application Insights
summary: How to monitor NServiceBus systems using Azure Application Insights
component: ServiceControl
reviewed: 2024-01-77
---

## Endpoint-level performance and health metrics

Azure Application Insights (App Insights) provides monitoring and alerting capabilities that can be leveraged to monitor the health of individual NServiceBus endpoints. It is possible to gather performance metrics like [Processing Time](/monitoring/metrics/definitions.md#metrics-captured-processing-time) and [Critical Time](/monitoring/metrics/definitions.md#metrics-captured-critical-time) as well as data on the number of immediate and delayed [retries](/nservicebus/recoverability).

For more details, see [this sample](/samples/open-telemetry/application-insights).

## Distributed tracing

It's possible to send tracing data to Application Insights for analysis and visualization. [This sample](/samples/open-telemetry/application-insights/) shows how to extend the NServiceBus pipeline with custom behaviors that publish trace data which later gets sent to Azure Monitor Application Insights.

## System-level health events

Integration events published by ServiceControl enable pushing system-level health events to 3rd party systems like Application Insights. [This sample](/samples/servicecontrol/azure-monitor-events) shows how to capture integration events triggered by ServiceControl and push them to Application Insights in the form of custom events.
