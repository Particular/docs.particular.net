---
title: Azure observability services
summary: Gives an overview of the observability options offered by Azure and how to use them with the Particular Service Platform
reviewed: 2024-05-27
related:
 - nservicebus/logging
 - monitoring/metrics
 - samples/open-telemetry
callsToAction: ['solution-architect', 'poc-help']
---

Azure’s main observability solution is [Azure Monitor](https://azure.microsoft.com/en-us/products/monitor), which can be used with the Particular Service Platform. Azure Monitor provides native monitoring, logging, tracing, alarming, and dashboards, capturing the [three main observability signals](https://opentelemetry.io/docs/concepts/signals/) into a single service.

:heavy_plus_sign: Pros:

- A single tool to visualize traces, metrics and logs emitted by an applications, infrastructure, and networks
- Fully managed service
- Tightly integrated with the Azure ecosystem enabling [auto-instrumentation for multiple environments and services](https://learn.microsoft.com/en-us/azure/azure-monitor/app/codeless-overview#supported-environments-languages-and-resource-providers)
- Customizable dashboards and automated alarms and actions
- Real-time telemetry

:heavy_minus_sign: Cons:

- There’s a learning curve due to the amount of features and capabilities
- [Pricing](https://azure.microsoft.com/en-us/pricing/details/monitor/) is based on the amount and the type of telemetry collected.
- Alerts and notifications may incur additional costs, as well as data archiving and restore operations.
- There is no free tier, the default pricing model is pay-as-you-go. Some data types, including [Azure Activity Logs](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/activity-log?tabs=powershell), are [free from data ingestion charges](https://docs.microsoft.com/en-us/azure/azure-monitor/logs/log-standard-columns#_isbillable).
- Extending the data retention period is subject to additional costs

The Particular Service Platform collects metrics in two forms:

- OpenTelemetry-based metrics, which can be collected by enabling OpenTelemetry and exporting the metrics to Amazon CloudWatch
- Custom metrics with [NServiceBus.Metrics](/monitoring/metrics) which can be exported to Azure Monitor Application Insights. NServiceBus version 8 and above supports [OpenTelemetry](https://opentelemetry.io/docs/instrumentation/net/) through [traces](/nservicebus/operations/opentelemetry.md#traces), [metrics](/nservicebus/operations/opentelemetry.md#metrics), and [logging](/nservicebus/operations/opentelemetry.md#logging). See the [OpenTelemetry samples](/samples/open-telemetry/) for details on how to integrate with standard OpenTelemetry exporters.

