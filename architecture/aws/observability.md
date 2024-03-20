---
title: AWS observability services
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'poc-help']
---

AWS offers several observability solutions which can be used with the Particular Service Platform. They provide native monitoring, logging, alarming, and dashboards with Amazon CloudWatch, tracing through AWS X-Ray, as well as other open-s&#111;urce based managed services, together providing [three pillars observability signals](https://opentelemetry.io/docs/concepts/signals/).

#### Amazon CloudWatch

[Amazon CloudWatch](https://aws.amazon.com/cloudwatch/) is a monitoring and observability service that allows one to collect and access performance and operational data in the form of logs and metrics on a single platform.

:heavy_plus_sign: Pros:

- A single tool to visualize metrics and logs emitted by the system
- Fully managed service
- Tightly integrated with other AWS services, such as AWS Lambda and Amazon EC2
- Supports querying data from multiple s&#111;urces, allowing one to monitor metrics on AWS, on premises or other clouds
- Customizable dashboards and automated alarms and actions
- Real-time telemetry

:heavy_minus_sign: Cons:

- Amazon CloudWatch does not support traces
- AWS CloudWatch offers a [free tier](https://aws.amazon.com/cloudwatch/pricing/), but costs may escalate with increases usage, requiring dedicated monitoring

The Particular Service Platform collects metrics in two forms:

- OpenTelemetry-based metrics, which can be collected by enabling OpenTelemetry and exporting the metrics to Amazon CloudWatch
- Custom metrics with NServiceBus.Metrics which can be exported to Amazon CloudWatch.

Try the Amazon CloudWatch sample for NServiceBus metrics and logs →

**TODO**:

1. Create a sample that uses NSB metrics, which integrates with Cloudwatch
    1. Collect NSB metrics, send to cloudwatch
    2. Collect [NLog logs with extensions.logging](/samples/logging/extensions-logging/), [send to Cloudwatch](https://docs.aws.amazon.com/prescriptive-guidance/latest/patterns/configure-logging-for-net-applications-in-amazon-cloudwatch-logs-by-using-nlog.html)

#### Amazon X-Ray

[Amazon X-Ray](https://aws.amazon.com/xray/) is a service that can collect trace data from the applications, providing insights that can help identify issues or bottlenecks that could benefit from optimization.

:heavy_plus_sign: Pros:

- A single tool to visualize application-level traces and infrastructure-level traces
- AWS X-Ray creates a service map using trace data
- Integrates with other AWS services, such as AWS Lambda, Amazon EC2, Amazon ECS and AWS Elastic Beanstalk
- Fully managed service

:heavy_minus_sign: Cons:

- AWS X-Ray offers a [free tier](https://aws.amazon.com/xray/pricing/), but costs may escalate with increases usage, requiring dedicated monitoring
- Pricing is based on the amount and the type of telemetry collected
- AWS X-Ray is designed to work within the AWS ecosystem
- AWS X-Ray’s `traceId` format differs from the [W3C format](https://www.w3.org/TR/trace-context/#trace-id), and requires mapping for compatibility reasons which should be considered when using [OpenTelemetry](/architecture/observability.md)

The Particular Service Platform allows observability tools to capture spans emitted by NServiceBus, providing insights into message processing, retries, and more.

&lt;link to presentation> [https:&#47;/particular.net/videos/message-processing-failed](https://particular.net/videos/message-processing-failed)

In this presentation, Laila Bougria discusses the need for distributed tracing in distributed systems, as well as the [ADOT collector (AWS Distro for OpenTelemetry Collector)](https://aws-otel.github.io/docs/getting-started/collector), AWS’ OpenTelemetry Collector implementation that simplifies the export of distributed traces from applications to AWS X-Ray, amongst others.

[Try the OpenTelemetry sample with export to Amazon X-Ray →](https://github.com/lailabougria/talks/tree/main/message-processing-failed-but-whats-the-root-cause/samples/aws)