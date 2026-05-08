---
title: IBM MQ Transport
summary: Integrate NServiceBus with IBM MQ for enterprise messaging on mainframe and distributed platforms
reviewed: 2026-03-23
component: IBMMQ
related:
 - samples/ibmmq/simple
 - samples/ibmmq/ebcdic
 - samples/ibmmq/polymorphic-events
redirects:
---

The IBM MQ transport allows NServiceBus endpoints to send and receive messages using IBM MQ. IBM MQ transport integrates NServiceBus with [IBM MQ](https://www.ibm.com/products/mq) using the [IBM MQ .NET client](https://www.nuget.org/packages/IBMMQDotnetClient/).

## Broker version compatibility

The transport requires IBM MQ 9.0 or later. IBM guarantees that [any supported MQ client can connect to any supported queue manager](https://www.ibm.com/docs/en/ibm-mq/9.4?topic=cci-compatibility-between-different-versions-mq-client-queue-manager), regardless of version.

The transport ships the IBM-provided [managed .NET client](https://www.ibm.com/docs/en/ibm-mq/9.4?topic=net-installing-mq-classes-standard) (`IBMMQDotnetClient`) and tracks the latest Continuous Delivery (CD) or Cumulative Security Update (CSU) release rather than pinning to a Long Term Support (LTS) fix pack. The managed client is wire-compatible with LTS queue managers, so customers on an LTS queue manager receive the newest client-side fixes through transport upgrades without needing to change their queue manager.

IBM MQ does not follow SemVer; it uses a four-segment `V.R.M.F` scheme with parallel LTS and CD streams. See the [IBM MQ versioning FAQ](https://www.ibm.com/support/pages/ibm-mq-faq-long-term-support-and-continuous-delivery-releases) for the full scheme and the [IBM product lifecycle page](https://www.ibm.com/support/pages/lifecycle/details/?q45=MQSeries,+MQ) for supported-version dates.

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions               |None, ReceiveOnly, SendsAtomicWithReceive
|Pub/Sub                    |Native
|Timeouts                   |Not natively supported
|Large message bodies       |Determined by queue manager `MAXMSGL` setting
|Scale-out                  |Competing consumer
|Scripted Deployment        |Supported via [CLI tool](operations-scripting.md)
|Installers                 |Optional
|Native integration         |[Supported](native-integration.md)
|OpenTelemetry tracing      |[Supported](observability.md)
|Case Sensitive             |Yes
|`TransactionScope` mode (distributed transactions) |No
|SSL/TLS encryption and certificate-based authentication |Yes
|Queue and topic names  |Limited to 48 characters
|Delayed delivery   |No. Requires an external timeout storage mechanism.
|Native message grouping    |Not yet implemented

## Configuring the endpoint

To use IBM MQ as the underlying transport:

snippet: ibmmq-config-basic

See [connection settings](connection-settings.md) for all available connection and configuration options.

## Message persistence

By default, all messages are sent as persistent, meaning they are  written to disk and survive queue manager restarts. Messages marked as [non-durable](/nservicebus/messaging/non-durable-messaging.md) are sent as non?persistent messages that stay in memory and offer higher throughput, but risks message loss if the queue manager stops unexpectedly.

> [!CAUTION]
> Non-persistent messages are lost if the queue manager restarts before they are consumed.
