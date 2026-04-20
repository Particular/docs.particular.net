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

Integrates NServiceBus with [IBM MQ](https://www.ibm.com/products/mq) using the [IBM MQ .NET client](https://www.nuget.org/packages/IBMMQDotnetClient/).

## Broker compatibility

The transport requires IBM MQ 9.0 or later. 

## Versioning

IBM MQ does not follow SemVer but uses their ["V.R.M.F" version numbers to identify LTS (Long term support), CD (Continious Delivery), CSU (Cumulative security update (CSU) and Fix packs](https://www.ibm.com/support/pages/ibm-mq-faq-long-term-support-and-continuous-delivery-releases).

The [IBM MQ management client version is backwards and forwards compatible](https://www.ibm.com/docs/en/ibm-mq/9.4.x?topic=cci-compatibility-between-different-versions-mq-client-queue-manager). For example, managed client 9.
A CD/CSU version 9.4.4.1 can connect to Queue Manager version 9.x



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

By default, all messages are sent as persistent, meaning they survive queue manager restarts. Messages marked as [non-durable](/nservicebus/messaging/non-durable-messaging.md) are sent as non-persistent for higher throughput.

> [!CAUTION]
> Non-persistent messages are lost if the queue manager restarts before they are consumed.
