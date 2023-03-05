---
title: Endpoint throughput counter tool FAQ
summary: Answers to frequently asked questions about the EndpointThroughputCounter tool
reviewed: 2023-01-25
related:
  - nservicebus/throughput-tool
---

This article covers questions that may arise about the [EndpointThroughputCounter tool](./).

## What does the tool do

The tool measures the number of endpoints used in a system, along with each endpoint's maximum daily throughout. After collecting this data, it produces a report in the directory where the tool was run.

## Why should I run the tool

The tool assists in gathering the information needed for licensing NServiceBus and the Particular Service Platform as required for production systems. Particular Software also uses the information in aggregate to better serve customer needs.

## Does the tool automatically submit the report to Particular Software

No, the tool does not automatically submit report data to Particular Software or any other party. The collected data is only used to generate a local report. That report can then be sent to Particular Software as needed.

## How do I choose a data collection method from all of the options

The data collection method to use depends on the message transport of the system being measured. Refer to the documentation on [Running the tool](/nservicebus/throughput-tool/#running-the-tool) to learn more about the data collection methods for different message transport configurations.

## Does the tool need to run on my production server

No, in most cases, the tool does not need to be run on a production server. Often, the tool can be run on a developer workstation that points to a production environment. The ability to do this depends on the configuration of the production system.

See [How does the tool measure throughput](#how-does-the-tool-measure-throughput) below for details on how the tool behaves when analyzing data from each collection method.

## What if I have a hybrid system with multiple message transports

The tool should be run for each environment, using the data collection method appropriate for that environment.

## How will running the tool affect my system

The tool was designed to be as lightweight and unintrusive as possible. Even in system configurations that require an extended runtime, queries are infrequent and most of the tool runtime is spent waiting. Thus, load from running the tool on a production system should be insignificant and likely to go unnoticed.

## How long will it take to run the tool

When using [Azure Service Bus](azure-service-bus.md) or [Amazon SQS](amazon-sqs.md), the tool will gather historical information from the cloud provider and then exit immediately.

When using any other data collection method, the query will run for approximately 24 hours in order to collect enough data to calculate the difference between initial and final readings.

## How does the tool measure throughput

The answer depends on the data collection mechanism.

### [Azure Service Bus](azure-service-bus.md)

The tool queries Azure Service Bus to get the queue names in the namespace. Then, for each queue, it queries the [Azure Monitor Metrics endpoint](https://learn.microsoft.com/en-us/rest/api/monitor/metrics/list?tabs=HTTP) to determine the maximum daily throughput from the previous 30 days for each queue.

The tool can be run from any workstation that can access the Azure Service Bus namespace.

### [Amazon SQS](amazon-sqs.md)

The tool queries AWS using the [ListQueues API](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ListQueues.html) to get the queue names in the namespace. Then, it queries the [AWS Cloudwatch GetMetricStatistics API](https://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/API_GetMetricStatistics.html) to gather daily throughput measurements for each queue.

The tool can be run from any workstation that can access AWS services.

### [RabbitMQ](rabbitmq.md)

The tool queries the [Management API](https://www.rabbitmq.com/management.html#http-api) to get a list of queues. Then, for each queue, it queries the [RabbitMQ monitoring queue metrics endpoint](https://www.rabbitmq.com/monitoring.html#queue-metrics) to retrieve the queue throughput at the beginning of the run. This query is repeated every 5 minutes (to guard against counter resets if the broker is restarted) for a period of 24 hours to determine daily throughput.

The tool can be run from any workstation that can access the RabbitMQ Management interface.

### [SQL Transport](sql-transport.md)

The tool queries SQL Server to find tables that look like queues based on their column structure, and gathers the throughput for each table. This process happens once when first executing the tool then again after 24 hours. Measurements are based on the differences in throughput between these first and second data gathers.

See the fully documented [SQL queries used by the tool](sql-transport.md#sql-queries) for review.

The tool can be run from any workstation with access to the database containing the queue tables.

### [ServiceControl](service-control.md)

The tool requires a [ServiceControl Monitoring](/servicecontrol/monitoring-instances/) instance to be used.

The tool queries the monitoring instance once every hour to get the per-endpoint throughput information for the last hour. It repeats this query 23 additional times to gather information for a 24-hour period.

For endpoints not configured to [send metrics data to ServiceControl](/monitoring/metrics/install-plugin.md), the tool will attempt to inspect audit information to find how many messages have been successfully processed each hour. However, this fallback mechanism is much more resource-intensive, so enabling metrics reporting on all endpoints is highly recommended.

## What if my system's queue names contain proprietary information I don't want on the report

System queue names can easily be masked on the report file. See [Masking private data](/nservicebus/throughput-tool/#masking-private-data) for details.

## Can I inspect the tool's source code

Yes, just like NServiceBus and all the Particular Service Platform tools, the [EndpointThroughputCounter source code](https://github.com/Particular/Particular.EndpointThroughputCounter) is available on GitHub.

## What should I do if I have problems running the tool

Particular Software can help. Open a [non-critical support case](https://particular.net/support) for assistance with running the tool.
