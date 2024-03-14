# AWS guidance

# Introduction

- [https://docs.particular.net/samples/aws/](https://docs.particular.net/samples/aws/)


## Messaging

### Amazon SNS

[https://aws.amazon.com/sns/](https://aws.amazon.com/sns/)

[Amazon Simple Notification Service](https://aws.amazon.com/sns/) (Amazon SNS)  is a fully managed web service that enables publishers to send messages to subscribers using various endpoint types. Publishers can communicate asynchronously with subscribers by sending messages to a topic, which is a logical access point and communication channel. Subscribers can receive messages using a supported endpoint type, such as Amazon Data Firehose, Amazon SQS, AWS Lambda, HTTP, email, mobile push notifications, and mobile text messages (SMS). Amazon SNS can be used for application-to-application (A2A) messaging, where messages are delivered from one application to another, or for application-to-person (A2P) messaging, where messages are delivered to customers with SMS texts, push notifications, and email. Amazon SNS provides features such as message filtering, batching, ordering, deduplication, encryption, and delivery retries to help developers build reliable, scalable, and secure applications.

:heavy_plus_sign: Pros:

- It provides instantaneous, push-based delivery of messages, which eliminates the need to poll for new information and updates.
- It has simple APIs and easy integration with applications, which reduces the development effort and complexity.
- It supports flexible message delivery over multiple transport protocols, such as HTTP, email, SMS, and mobile push notifications.
- It has an inexpensive, pay-as-you-go model with no up-front costs, which lowers the operational expenses.

:heavy_minus_sign: Cons:

- It is not suitable for ordered message processing, as it does not guarantee the order of delivery or the number of deliveries for each message.
- It has limitations in fine-grained control over retry policies, as it only allows configuring the number of retries and the delay between retries for each endpoint type.
- It may incur additional costs for using other AWS services, such as SQS, Lambda, or S3, to process or store the messages delivered by SNS.
- It may have compatibility issues with some third-party services or applications, as it only supports a subset of protocols and format.

---
Call to action??
---

#### When to use Amazon SNS?

Amazon SNS should be used when you are looking for a solution to achieve higher decoupling between your publishers and your topic subscribers.

### Amazon SQS

[https://aws.amazon.com/sqs/](https://aws.amazon.com/sqs/)

[Amazon Simple Queue Service](https://aws.amazon.com/sqs/) (Amazon SQS) is a scalable and managed message queuing service provided by AWS that enables the decoupling of application components. This service is designed to help developers build robust, distributed applications with decoupled components, making it easier to manage traffic, system failures, and complex workflows. SQS provides a reliable and secure platform for sending, storing, and receiving messages at any volume, thereby streamlining the process of building and scaling microservices, distributed systems, and serverless applications.

:heavy_plus_sign: Pros:

- Highly scalable with an ability to handle large volumes of messages automatically
- Highly reliable as messages are locked during processing to prevent loss and enable  concurrency.
- Easy integration with other AWS services
- Enables decoupling and scalability of microservices, distributed systems and serverless applications
- Cost effective as charges are based on usage with no upfront costs easing up the need to do capacity planning and pre-provisioning
- Secure as it allows you to send sensitive data between applications either by managing your keys using AWS Key management (AWS KMS) and by using Amazon SQS server side encryption (SSE)
- Durable as messages are stored on multiple servers
- Supports message deduplication
- Queues can be fully customizable

:heavy_minus_sign: Cons:

- Limited message size (256Kb per message). NServiceBus mitigates this by allowing you to take advantage, in a seamless way, of S3 to work with larger payloads. For more information review the documentation for the [Amazon SQS transport topology](https://docs.particular.net/transports/sqs/topology#s3) and [Amazon SQS configuration options](https://docs.particular.net/transports/sqs/configuration-options).
- Limited control over retry policies which forces delegation of handling retries to consumers increasing the overall complexity of the system
- Messages are only visible for a configurable period of time which can lead to challenges when failures occur
- As your system grows in complexity, managing a large number of queues can be challenging
- Even with FIFO (First-In-First-Out) queues, strict message ordering can be a challenge increasing complexity and impact system performance
- SQS supports a subset of protocols and formats which can originate compatibility issues with third party applications

---
Call to action

- [Simple AmazonSQS Transport usage • Amazon SQS Transport Samples • Particular Docs](https://docs.particular.net/samples/aws/sqs-simple/) (include DataBus properties info)
- [https://docs.particular.net/samples/aws/sagas-lambda-aurora/](https://docs.particular.net/samples/aws/sagas-lambda-aurora/)
---

#### When to use Amazon SQS?

Use Amazon SQS when you want a solution:

- To decouple your microservices and facilitate asynchronous communication between them
- Where you don’t need replay your events or commands to understand current state of your entities/processes
- When you need to manage workloads that require data processing in batches
- When you need to send notifications or alerts within an application
- When you need to do data ingestion and use it as a buffer for incoming requests
- You’d want to fanout and send identical copies of a message to multiple queues in parallel - combined with Amazon SNS.

## Observability

AWS offers several observability solutions which can be used with the Particular Service Platform. They provide native monitoring, logging, alarming, and dashboards with Amazon CloudWatch, tracing through AWS X-Ray, as well as other open-source based managed services, together providing [three pillars observability signals](https://opentelemetry.io/docs/concepts/signals/).

#### Amazon CloudWatch

[Amazon CloudWatch]([https://aws.amazon.com/cloudwatch/](https://aws.amazon.com/cloudwatch/)) is a monitoring and observability service that allows you to collect and access performance and operational data in the form of logs and metrics on a single platform.

:heavy_plus_sign: Pros:

- A single tool to visualize metrics and logs emitted by your system
- Fully managed service
- Tightly integrated with other AWS services, such as AWS Lambda and Amazon EC2
- Supports querying data from multiple sources, allowing you to monitor metrics on AWS, on premises or other clouds
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
    2. Collect [NLog logs with extensions.logging](https://docs.particular.net/samples/logging/extensions-logging/), [send to Cloudwatch](https://docs.aws.amazon.com/prescriptive-guidance/latest/patterns/configure-logging-for-net-applications-in-amazon-cloudwatch-logs-by-using-nlog.html)

#### Amazon X-Ray

[Amazon X-Ray]([https://aws.amazon.com/xray/](https://aws.amazon.com/xray/)) is a service that can collect trace data from your applications, providing insights that can help identify issues or bottlenecks that could benefit from optimization.

:heavy_plus_sign: Pros:

- A single tool to visualize application-level traces and infrastructure-level traces
- AWS X-Ray creates a service map using trace data
- Integrates with other AWS services, such as AWS Lambda, Amazon EC2, Amazon ECS and AWS Elastic Beanstalk
- Fully managed service

:heavy_minus_sign: Cons:

- AWS X-Ray offers a [free tier](https://aws.amazon.com/xray/pricing/), but costs may escalate with increases usage, requiring dedicated monitoring
- Pricing is based on the amount and the type of telemetry collected
- AWS X-Ray is designed to work within the AWS ecosystem
- AWS X-Ray’s `traceId` format differs from the [W3C format](https://www.w3.org/TR/trace-context/#trace-id), and requires mapping for compatibility reasons which should be considered when using [OpenTelemetry](link-to-otel-section-under-observability)

The Particular Service Platform allows you to capture spans emitted by NServiceBus, providing insights into message processing, retries, and more.

&lt;link to presentation> [https://particular.net/videos/message-processing-failed](https://particular.net/videos/message-processing-failed)

In this presentation, Laila Bougria discusses the need for distributed tracing in distributed systems, as well as the [ADOT collector (AWS Distro for OpenTelemetry Collector)](https://aws-otel.github.io/docs/getting-started/collector), AWS’ OpenTelemetry Collector implementation that simplifies the export of distributed traces from applications to AWS X-Ray, amongst others.

[Try the OpenTelemetry sample with export to Amazon X-Ray →](https://github.com/lailabougria/talks/tree/main/message-processing-failed-but-whats-the-root-cause/samples/aws)

### Concepts - Observability (or monitoring)

Observability is the idea that we should be able to understand our system’s behavior by investigating telemetry information, without the need to change any code or configuration, or investigate any system-specific data stores which may not be accessible and/or contain sensitive information. By collecting telemetry data from our systems, we’re able to query and analyze that information, with the end-goal of extracting actionable insights that can help us improve our systems.

Observability can help address multiple concerns in a system:

#### Root-cause analysis

Distributed systems are complex system landscapes where multiple components interact with each other through different mechanisms in different time frames (asynchronous and synchronous communication). This makes it a lot harder to pinpoint the root cause of failures, as multiple components and infrastructure may be involved. It’s quite common in message-based systems, that the cause of a failure observed in one component, is caused by another component further upstream. By collecting traces and logs from our systems, we can gather insights into what’s happening in the system and make it easier to pinpoint the root cause of failures.

#### Health monitoring

Is my system healthy and operable? The ability to answer this question becomes more complex in distributed systems as our system is composed of multiple components that operate autonomously. This requires us to pay more attention to the health and operability of individual components. By emitting telemetry (in the form of metrics or otherwise) from all components that make up our systems, we can understand the health of our system and gain insights into individual components that may be struggling.

#### Performance monitoring

One component of performance monitoring that is harder to understand in production environments, is latency. Latency can severely affect the performance of our overall system, even when individual components have been performance-tested during development. Emitting telemetry, specifically traces, can help us gain insight into the entire request execution, across all the relevant components in the system.

#### OpenTelemetry

With the increasing need for observability, multiple standards started emerging in the industry to standardize the collection of telemetry data from distributed systems, including Google’s [OpenCensus standard](https://opencensus.io/) and [OpenTracing](https://opentracing.io/). In an effort to standardize in a cross-platform, cross-runtime, cross-language manner, the [Cloud Native Computing Foundation](https://www.cncf.io/) set up the [OpenTelemetry project](https://opentelemetry.io), effectively merging the OpenCensus and OpenTracing standards into one. Its goal is to help standardize how we instrument, generate, collect, and export telemetry data from our applications, by providing multiple tools and SDK for multiple languages and platforms. Most observability vendors have adopted the OpenTelemetry standard, enabling customers to store their telemetry data in a vendor-agnostic format.

Video: Message processing failed, but what’s the root cause? https://www.youtube.com/watch?v=U8Aame0akl4&pp=ygUSbGFpbGEgYm91Z3JpYSBvc2xv

#### The Particular Service Platform

The Particular Service Platform offers multiple capabilities that allow you to observe the message flows that are occurring in the system. ServiceInsight has an endpoint explorer view, allowing you to understand all components that are sending and receiving messages in the system. More importantly, ServiceInsight offers multiple views that offer insights into message and [saga flows](https://docs.particular.net/architecture/workflows#orchestration-implementing-orchestrated-workflows), based on audit messages. These message-based conversations that are occurring in the systems, are visualized in [flow diagrams](https://docs.particular.net/serviceinsight/#flow-diagram), [sequence diagrams](https://docs.particular.net/serviceinsight/#sequence-diagram), and [saga views](https://docs.particular.net/serviceinsight/#the-saga-view).
