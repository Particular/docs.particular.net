---
title: Integrating with Azure Application Insights
summary: How to monitor NServiceBus systems using Azure Application Insights
component: ServiceControl
reviewed: 2021-04-21
---

## Endpoint-level performance and health metrics

Azure Application Insights (App Insights) provides monitoring and alerting capabilities that can be leveraged to monitor the health of individual NServiceBus endpoints. It is possible to gather performance metrics like [Processing Time](/monitoring/metrics/definitions.md#metrics-captured-processing-time) and [Critial Time](/monitoring/metrics/definitions.md#metrics-captured-critical-time) as well as data on number of immediate and delayed [retries](/nservicebus/recoverability).

For more details, see [this sample](/samples/logging/application-insights).

## Distributed tracing

It's possible to send tracing data to Application Insights for analysis and visualization. [This sample](/samples/tracing) shows how to extend the NServiceBus pipeline with custom behaviors that publish trace data which later gets sent to Azure Monitor Application Insights.

## System-level health events

Integration events published by ServiceControl enable pushing system-level health events to 3rd party systems like Application Insights. [This sample](/samples/servicecontrol/azure-monitor-events) shows how to capture integration events triggered by ServiceControl and push them to Application Insights in form of custom events.
