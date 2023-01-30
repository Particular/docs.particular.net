---
title: Endpoint throughput tool frequently asked questions
summary: Answers to frequently asked questions related to the EndpointThroughputCounter
reviewed: 2023-01-25
related:
  - nservicebus/throughput-tool
---

This article covers questions that may arise when running the [EndpointThroughputCounter tool](./).

## What does the tool do

The tool inspects a production system to measure the number of endpoints in use and the maximum daily throughput of each endpoint. After gathering this information, it outputs a report to a local file in the directory where the tool was run, which can be sent to Particular Software.

## Does the tool automatically submit the report

No. The tool does not transmit report data to Particular Software or any other party, the data collected is only used to generate the local report.

## Why should I run the tool

The tool is provided to assist in gathering information needed for licensing NServiceBus and the Particular Service Platform. Particular Software also uses the information in aggregate to better serve customers' needs.

## Does the tool need to run on my production server

In most cases, no. The exact access required depends upon the configuration of the production system. In many scenarios, the tool can be run on a developer workstation.

See [How does the tool measure throughput](#how-does-the-tool-measure-throughput) below for details on how the tool behaves when analyzing each data collection option.


## How do I decide which data collection option to use

Refer to the [Running the tool](/nservicebus/throughput-tool/#running-the-tool) section of the documentation to select the correct data collection option based on the message transport used by the system.

## What if I have a hybrid system with multiple message transports

The tool should be run for each environment, using the data collection option appropriate for that environment.

## What should I do if I have problems running the EndpointThroughputCounter

Open a [non-critical support case](https://particular.net/support) with Particular Software.

## How will running the tool affect my system

The tool was designed to be as lightweight and non-intrusive as possible. The load from running the tool on a production system should be insignificant and likely unnoticeable. In system configurations that require an extended runtime, queries are infrequent, and most of the tool runtime is spent waiting.

## How long will it take to run the tool

If using [Azure Service Bus](azure-service-bus.md) or [Amazon SQS](amazon-sqs.md), the tool will gather historical information from the cloud provider and then exit immediately.

If using any other data collection mechanism, the query will run for approximately 24 hours in order to calculate the difference between initial and final readings.

## How does the tool measure throughput
The answer depends on the data collection mechanism.

##### [Azure Service Bus](azure-service-bus.md)

The tool first queries Azure Service Bus to get the queue names in the namespace. Then, for each queue, it queries the [Azure Monitor Metrics endpoint](https://learn.microsoft.com/en-us/rest/api/monitor/metrics/list?tabs=HTTP) to determine the maximum daily throughput from the previous 30 days for each queue.

The tool can be run from any workstation that can access the Azure Service Bus namespace.

##### [Amazon SQS](amazon-sqs.md)

The tool queries AWS using the [ListQueues api](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ListQueues.html) to find all the queue names. Then it queries each queue using the [AWS Cloudwatch GetMetricStatistics Api](https://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/API_GetMetricStatistics.html) to gather daily throughput measurements for each queue.

The tool can be run from any workstation that can access AWS services.

##### [RabbitMQ](rabbitmq.md)

The tool queries the [Management API](https://www.rabbitmq.com/management.html#http-api) to get a list of queues, Then, for each queue, it queries the [RabbitMQ monitoring queue metrics endpoint](https://www.rabbitmq.com/monitoring.html#queue-metrics) to retrieve the queue throughput at the beginning of the run. This query is repeated every 5 minutes (to guard against counter resets if the broker is restarted) for a period of 24 hours to determine daily throughput.

The tool can be run from any workstation that can access the RabbitMQ Management interface.

##### [SQL Transport](sql-transport.md)

The tool queries SQL Server to find tables that look like queues based on their column structure, and gathers the throughput for each table. This process happens at once when first executing the tool then again after 24 hours. Measurements are then made based on the difference in throughput between the first and second process of gathering throughput.

The [SQL queries used by the tool](sql-transport.md#sql-queries) are documented for inspection by a database administrator.

The tool can be run from any workstation with access to the database containing the queue tables.

##### [ServiceControl](service-control.md)

The tool requires a [ServiceControl Monitoring] instance to be used.

The tool queries the monitoring instance once per hour to get the per-endpoint throughput information for the last hour. It repeats this query 23 additional times to gather information for a 24-hour period.

For endpoints not configured to [send metrics data to ServiceControl](/monitoring/metrics/install-plugin.md), the tool attempts to inspect Audit information to find how many messages have been successfully processed each hour. This fallback mechanism is much more resource-intensive, so it is recommended to ensure all endpoints have metrics reporting enabled.

## Some queue names contain proprietary information

These names can be masked in the report file. See [Masking private data](/nservicebus/throughput-tool/#masking-private-data).

## Can I inspect the tool's source code

Yes, just like NServiceBus and all the Particular Service Platform tools, the source is [available on GitHub](https://github.com/Particular/Particular.EndpointThroughputCounter).