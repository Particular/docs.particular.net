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
- AWS X-Ray’s `traceId` format differs from the [W3C format](https://www.w3.org/TR/trace-context/#trace-id), and requires mapping for compatibility reasons which should be considered when using [OpenTelemetry](#opentelemetry)

The Particular Service Platform allows observability tools to capture spans emitted by NServiceBus, providing insights into message processing, retries, and more.

&lt;link to presentation> [https:&#47;/particular.net/videos/message-processing-failed](https://particular.net/videos/message-processing-failed)

In this presentation, Laila Bougria discusses the need for distributed tracing in distributed systems, as well as the [ADOT collector (AWS Distro for OpenTelemetry Collector)](https://aws-otel.github.io/docs/getting-started/collector), AWS’ OpenTelemetry Collector implementation that simplifies the export of distributed traces from applications to AWS X-Ray, amongst others.

[Try the OpenTelemetry sample with export to Amazon X-Ray →](https://github.com/lailabougria/talks/tree/main/message-processing-failed-but-whats-the-root-cause/samples/aws)

### Concepts - Observability (or monitoring)

Observability is the idea that one should be able to understand the system’s behavior by investigating telemetry information, without the need to change any code or configuration, or investigate any system-specific data stores which may not be accessible and/or contain sensitive information. By collecting telemetry data from systems, one is able to query and analyze that information, with the end-goal of extracting actionable insights that can help improve one's systems.

Observability can help address multiple concerns in a system:

#### Root-cause analysis

Distributed systems are complex system landscapes where multiple components interact with each other through different mechanisms in different time frames (asynchronous and synchronous communication). This makes it a lot harder to pinpoint the root cause of failures, as multiple components and infrastructure may be involved. It’s quite common in message-based systems, that the cause of a failure observed in one component, is caused by another component further upstream. By collecting traces and logs from systems, one can gather insights into what’s happening in the system and make it easier to pinpoint the root cause of failures.

#### Health monitoring

Is my system healthy and operable? The ability to answer this question becomes more complex in distributed systems as the system is composed of multiple components that operate autonomously. This requires more attention to be paid to the health and operability of individual components. By emitting telemetry (in the form of metrics or otherwise) from all components that make up the systems, one can understand the health of the system and gain insights into individual components that may be struggling.

#### Performance monitoring

One component of performance monitoring that is harder to understand in production environments, is latency. Latency can severely affect the performance of the overall system, even when individual components have been performance-tested during development. Emitting telemetry, specifically traces, can help one gain insight into the entire request execution, across all the relevant components in the system.

#### OpenTelemetry

With the increasing need for observability, multiple standards started emerging in the industry to standardize the collection of telemetry data from distributed systems, including Google’s [OpenCensus standard](https://opencensus.io/) and [OpenTracing](https://opentracing.io/). In an effort to standardize in a cross-platform, cross-runtime, cross-language manner, the [Cloud Native Computing Foundation](https://www.cncf.io/) set up the [OpenTelemetry project](https://opentelemetry.io), effectively merging the OpenCensus and OpenTracing standards into one. Its goal is to help standardize how one instruments, generates, collects, and exports telemetry data from applications, by providing multiple tools and SDK for multiple languages and platforms. Most observability vendors have adopted the OpenTelemetry standard, enabling customers to store their telemetry data in a vendor-agnostic format.

Video: [Message processing failed, but what’s the root cause?](https://www.youtube.com/watch?v=U8Aame0akl4&pp=ygUSbGFpbGEgYm91Z3JpYSBvc2xv)

#### The Particular Service Platform

The Particular Service Platform offers multiple capabilities that allow one to observe the message flows that are occurring in the system. ServiceInsight has an endpoint explorer view, allowing one to understand all components that are sending and receiving messages in the system. More importantly, ServiceInsight offers multiple views that offer insights into message and [saga flows](/architecture/workflows#orchestration-implementing-orchestrated-workflows), based on audit messages. These message-based conversations that are occurring in the systems, are visualized in [flow diagrams](/serviceinsight/#flow-diagram), [sequence diagrams](/serviceinsight/#sequence-diagram), and [saga views](/serviceinsight/#the-saga-view).
