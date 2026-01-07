---
title: AWS messaging services
summary: Describes the AWS messaging options available for the Particular Service Platform
reviewed: 2026-01-07
callsToAction: ['solution-architect', 'poc-help']
---

AWS offers [multiple messaging services](https://aws.amazon.com/messaging/). The Particular Service Platform offers messaging support within AWS through [Amazon SQS](https://aws.amazon.com/sqs/) or [SQL Server-based transport](#sql-server-transport).

## Amazon SQS

[Amazon Simple Queue Service](https://aws.amazon.com/sqs/) (Amazon SQS) is a scalable and managed message queuing service provided by AWS that enables the decoupling of application components. This service is designed to help developers build robust distributed applications, making it easier to manage traffic, system failures, and complex workflows. Amazon SQS provides a reliable and secure platform for sending, storing, and receiving messages at any volume, thereby streamlining the process of building and scaling microservices, distributed systems, and serverless applications.

:heavy_plus_sign: Pros:

- High scalability
- High reliability
- Easy integration with other AWS services
- Sensitive data is secured through server-side encryption (SSE)
- Cost-effective: charges are based on usage, reducing the need for capacity planning and pre-provisioning

:heavy_minus_sign: Cons:

- Limited message size (256Kb per message)
- Limited control over retry policies forces delegation of handling retries to consumers which can increase the overall complexity of the system
- SQS supports only a subset of protocols and formats which can cause compatibility issues with third-party applications
- No local store-and-forward mechanism available

NServiceBus addresses some of these limitations:
- Limited message size: NServiceBus allows the use of S3 to work with larger payloads. For more information, review the documentation for the [Amazon SQS transport topology](/transports/sqs/topology.md#s3) and [Amazon SQS configuration options](/transports/sqs/configuration-options.md).
- Limited control over retry policies: NServiceBus provides customizable [immediate](/nservicebus/recoverability/configure-immediate-retries.md) and [delayed](/nservicebus/recoverability/configure-delayed-retries.md) retry policies.

[**Try the SQS transport sample →**](/samples/aws/sqs-simple/)

### When to use the Amazon SQS transport

The Amazon SQS transport should be considered the default choice for AWS-based systems. Alternatives should be considered only if SQS cannot be used for organizational reasons. The Amazon SQS transport uses Amazon SNS and S3 under the hood.

## SQL Server transport

SQL Server transport is an NServiceBus feature that can use existing SQL Server databases as feature-complete message queues.

:heavy_plus_sign: Pros:

- Runs on infrastructure which often already exists
- Strong transaction integration with business data operations
- Runs on cloud-hosted and on-premises SQL Server-compatible data stores (including Amazon RDS)
- Arbitrary message sizes
- Allows for [exactly-once processing](https://particular.net/blog/what-does-idempotent-mean) if business data and message data are in the same database
- Ease of backup and recovery as business data and messages are backed up in the same database

:heavy_minus_sign: Cons:

- More expensive and laborious to scale
- Impacts overall database performance
- Lower message throughput compared to specialized message queuing technologies

[**Try the SQL transport sample →**](/samples/sqltransport/simple/)

### When to use SQL Server transport

Consider using SQL transport if an existing application already uses a SQL Server-compatible data store and only a limited amount of messaging is being introduced. SQL transport can be a good stepping-stone when introducing messaging into an existing system without the introduction of new infrastructure.
