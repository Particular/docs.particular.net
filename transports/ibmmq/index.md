---
title: IBM MQ Transport
summary: Integrate NServiceBus with IBM MQ for enterprise messaging on mainframe and distributed platforms
reviewed: 2026-02-19
component: IBMMQ
related:
redirects:
---

Provides support for sending messages over [IBM MQ](https://www.ibm.com/products/mq) using the [IBM MQ .NET client](https://www.nuget.org/packages/IBMMQDotnetClient/).

## Broker compatibility

The transport requires IBM MQ 9.0 or later. 

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions               |None, ReceiveOnly, SendsAtomicWithReceive
|Pub/Sub                    |Native
|Timeouts                   |Not natively supported
|Large message bodies       |Up to 100 MB (configurable)
|Scale-out                  |Competing consumer
|Scripted Deployment        |Supported via [CLI tool](operations-scripting.md)
|Installers                 |Optional
|Native integration         |[Supported](native-integration.md)
|Case Sensitive             |Yes
|`TransactionScope` mode (distributed transactions) |No
|SSL/TLS encryption and certificate-based authentication |Yes
|Queue and topic names  |Limited to 48 characters
|Delayed delivery   |No. Requires an external timeout storage mechanism.

## Configuring the endpoint

To use IBM MQ as the underlying transport:

snippet: ibmmq-config-basic

See [connection settings](connection-settings.md) for all available connection and configuration options.