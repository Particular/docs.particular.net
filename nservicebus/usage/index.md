---
title: Measuring system usage
summary: Measure the usage of an NServiceBus system.
reviewed: 2024-05-08
related:
  - servicepulse/usage
  - nservicebus/throughput-tool
---

There are two methods provided for measuring usage of an NServiceBus system:

- [Using ServicePulse](#using-servicepulse)
- [Standalone endpoint throughput tool](#standalone-endpoint-throughput-tool)

> [!NOTE]
> Requirements for both of the usage measuring methods:
>
> - NServiceBus Transport: [Azure Service Bus](/transports/azure-service-bus), [RabbitMQ](/transports/rabbitmq), [Amazon SQS](/transports/sqs), [SqlServer](/transports/sql) OR
> - NServiceBus Transport: [MSMQ](/transports/msmq/) or [Azure Storage Queues](/transports/azure-storage-queues/) with [Auditing](./../operations/auditing.md) or [Monitoring](./../../monitoring/metrics) enabled on all NServiceBus endpoints

The output of the measuring methods is a usage report containing NServiceBus endpoint count and throughput summary. The report is generated as a JSON file which needs to be sent to Particular upon request, usually at license renewal time.

## Using ServicePulse

The recommended method of measuring the usage of an NServiceBus system is via [ServicePulse](/servicepulse/usage.md).

> [!NOTE]
> This method requires ServicePulse version 1.39 or later, and ServiceControl version 5.3 or later.

Measuring throughput with ServicePulse offers the following advantages over the standalone throughput tool:

- ability to [view the system usage](/servicepulse/usage.md#viewing-usage-summary) at any time
- ability to optionally [specify if a detected queue should not be included in license pricing](/servicepulse/usage.md#setting-an-endpoint-type)
- no requirement to run additional tools - [the report can be generated directly from ServicePulse](/servicepulse/usage.md#generating-a-usage-report)
- improved endpoint identification

## Standalone Endpoint throughput tool

Customers who are not able to use a version of ServicePulse that supports throughput measurement can use the [endpoint throughput counter tool](./../throughput-tool) to measure their system usage.

This is a standalone tool that can typically be installed on a [user's workstation](/nservicebus/throughput-tool/faq.md#does-the-tool-need-to-run-on-my-production-server).
