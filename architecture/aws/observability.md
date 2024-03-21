---
title: AWS observability services
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'poc-help']
---

AWS offers several observability solutions which can be used with the Particular Service Platform. They provide native monitoring, logging, alarming, and dashboards with Amazon CloudWatch, tracing through AWS X-Ray, as well as other open-source based managed services. Together these provide the three pillars of [observability signals](https://opentelemetry.io/docs/concepts/signals/).

## Amazon CloudWatch

[Amazon CloudWatch](https://aws.amazon.com/cloudwatch/) is a monitoring and observability service that allows one to collect and access performance and operational data in the form of logs and metrics on a single platform.

:heavy_plus_sign: Pros:

- Single tool to visualize metrics and logs emitted by the system
- Fully managed service
- Tightly integrated with other AWS services
- Supports querying data from multiple sources
- Customizable dashboards with automated alarms and remediating actions
- Real-time telemetry

:heavy_minus_sign: Cons:

- Does not support traces
- [Costs](https://aws.amazon.com/cloudwatch/pricing/) can escalate with increased usage

The Particular Service Platform collects metrics in two forms:

1. OpenTelemetry-based metrics - can be collected by [enabling OpenTelemetry](/nservicebus/operations/opentelemetry.md) and exporting the metrics to Amazon CloudWatch.
1. Custom metrics with `NServiceBus.Metrics` - can be exported to Amazon CloudWatch - TODO: Link this to TODO2 below.

Try the Amazon CloudWatch sample for OpenTelemetry → TODO: Link this to TODO1 below.

**TODO**:

1. Create a sample that uses Otel and integrates with CloudWatch
1. Create a sample that uses NSB metrics, which integrates with CloudWatch
    1. Collect NSB metrics, send to cloudwatch
    2. Collect [NLog logs with extensions.logging](/samples/logging/extensions-logging/), [send to Cloudwatch](https://docs.aws.amazon.com/prescriptive-guidance/latest/patterns/configure-logging-for-net-applications-in-amazon-cloudwatch-logs-by-using-nlog.html)

## Amazon X-Ray

[Amazon X-Ray](https://aws.amazon.com/xray/) is a service that can receive trace data from applications. The trace data can provide insights that can help identify performance bottlenecks, edge case errors, and other hard to detect issues.

:heavy_plus_sign: Pros:

- Single tool to visualize application-level and infrastructure-level traces
- Creates a service map using trace data
- Tightly integrated with other AWS services
- Fully managed service

:heavy_minus_sign: Cons:

- [Costs](https://aws.amazon.com/xray/pricing/) can escalate with increased usage
- [OpenTelemetry](/architecture/observability.md) compatibility is not guaranteed due to `traceId` format differences from the [W3C specification](https://www.w3.org/TR/trace-context/#trace-id)

The Particular Service Platform allows observability tools to capture spans emitted by NServiceBus, providing insights into how messages are being processed.

[Try the OpenTelemetry sample with export to Amazon X-Ray →](https://github.com/lailabougria/talks/tree/main/message-processing-failed-but-whats-the-root-cause/samples/aws)

## Additional resources

[Message processing failed... But what's the root cause?](https://particular.net/videos/message-processing-failed)

In this presentation, Laila Bougria discusses the need for distributed tracing in distributed systems, as well as the [ADOT collector (AWS Distro for OpenTelemetry Collector)](https://aws-otel.github.io/docs/getting-started/collector), AWS’ OpenTelemetry Collector implementation that simplifies the export of distributed traces from applications to AWS X-Ray, amongst others.