---
title: AWS observability services
summary: An overview of the observability options offered by AWS and how to use them with the Particular Service Platform
reviewed: 2026-01-12
callsToAction: ['solution-architect', 'poc-help']
---

AWS offers several observability solutions that can be used with the Particular Service Platform. They provide native monitoring, logging, alarming, and dashboards with Amazon CloudWatch, tracing through AWS X-Ray, and other open-source-based managed services. Together, these provide the three pillars of [observability signals](https://opentelemetry.io/docs/concepts/signals/).

## Amazon CloudWatch

[Amazon CloudWatch](https://aws.amazon.com/cloudwatch/) is a monitoring and observability service that allows the collection and access of performance and operational data in the form of logs and metrics on a single platform.

:heavy_plus_sign: Pros:

- Single tool to visualize metrics and logs emitted by the system
- Fully managed service
- Tightly integrated with other AWS services
- Supports querying data from multiple sources to monitor metrics from AWS, on-premises, or other cloud providers
- Customizable dashboards with automated alarms and remediating actions
- Real-time telemetry

:heavy_minus_sign: Cons:

- Does not support traces
- [Costs](https://aws.amazon.com/cloudwatch/pricing/) can escalate with increased usage

The Particular Service Platform collects metrics in two forms:

1. OpenTelemetry-based metrics - can be collected by [enabling OpenTelemetry](/nservicebus/operations/opentelemetry.md) and exporting the metrics to Amazon CloudWatch.
1. Custom metrics with `NServiceBus.Metrics` - can be exported to Amazon CloudWatch.

## Amazon X-Ray

[Amazon X-Ray](https://aws.amazon.com/xray/) is a service that can receive trace data from applications. The trace data can provide insights to help identify the root cause of failures, hard-to-detect issues or side effects, and performance bottlenecks.

:heavy_plus_sign: Pros:

- Single tool to visualize application-level and infrastructure-level traces
- Creates a service map using trace data
- Tightly integrated with other AWS services
- Fully managed service

:heavy_minus_sign: Cons:

- [Costs](https://aws.amazon.com/xray/pricing/) can escalate with increased usage

The Particular Service Platform allows observability tools to capture spans emitted by NServiceBus, providing insights into how messages are being processed.

[**Try the OpenTelemetry sample with export to Amazon X-Ray →**](https://github.com/lailabougria/talks/tree/main/message-processing-failed-but-whats-the-root-cause/samples/aws)

## Additional resources

[Message processing failed... But what's the root cause?](https://particular.net/videos/message-processing-failed)

In this presentation, Laila Bougria discusses the need for distributed tracing in distributed systems, as well as the [ADOT collector (AWS Distro for OpenTelemetry Collector)](https://aws-otel.github.io/docs/getting-started/collector), AWS’ OpenTelemetry Collector implementation that simplifies exporting distributed traces from applications to AWS X-Ray, among others.
