---
title: Measuring system usage
summary: Measure the usage of an NServiceBus system.
reviewed: 2024-05-08
related:
  - servicepulse/usage
  - nservicebus/throughput-tool
---

There are two methods available for measuring usage of an NServiceBus system:

- [Using ServicePulse](#using-servicepulse) (**preferred method**)
- [Standalone Endpoint Throughput tool](#standalone-endpoint-throughput-tool)

Both methods will generate a usage report containing NServiceBus endpoint count and throughput summary. The report is saved as a file onto the local machine, which needs to be sent to Particular upon request, usually at license renewal time.

## Using ServicePulse

The recommended method of measuring the usage of an NServiceBus system is via ServicePulse.

Measuring usage with ServicePulse offers the following advantages over the standalone endpoint throughput tool:

- ability to [view the system usage](/servicepulse/usage.md#viewing-usage-summary) at any time
- ability to [specify if a detected queue should not be included in license pricing](/servicepulse/usage.md#setting-an-endpoint-type)
- incorporated into the existing Particular Platform - [the report can be generated directly from ServicePulse](/servicepulse/usage.md#download-a-usage-report)
- improved endpoint detection for endpoints that have Monitoring or Audit enabled
- once setup, there's no more work required, just a button press once a year to generate the usage report

This method requires ServicePulse version 1.40 or later, and ServiceControl version 5.4 or later.
Additionally, If using RabbitMQ broker, version 3.10.0 or higher is required.

To learn more about this method, read the [ServicePulse Usage documentation](/servicepulse/usage.md). 

## Standalone Endpoint Throughput tool

Customers who are not able to use the preferred method above, can use the Endpoint Throughput tool to measure their system usage.

This is a standalone tool that is run on demand, and can typically be installed on a [user's workstation](/nservicebus/throughput-tool/faq.md#does-the-tool-need-to-run-on-my-production-server).

To learn more about this method, read the [Endpoint Throughput tool documentation](./../throughput-tool). 

## Requirements

If measuring usage for NServiceBus endpoints using [MSMQ](/transports/msmq/) or [Azure Storage Queues](/transports/azure-storage-queues/) transport, then [Auditing](./../operations/auditing.md) or [Monitoring](./../../monitoring/metrics) needs to be enabled on all NServiceBus endpoints.
