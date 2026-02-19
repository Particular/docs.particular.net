---
title: IBM MQ Transport
summary: Integrate NServiceBus with IBM MQ for enterprise messaging on mainframe and distributed platforms
reviewed: 2026-02-19
component: IbmMq
related:
redirects:
---

Provides support for sending messages over [IBM MQ](https://www.ibm.com/products/mq) using the [IBM MQ .NET client](https://www.nuget.org/packages/IBMMQDotnetClient/).

## Broker compatibility

The transport requires IBM MQ 9.0 or later. It has been tested with:

- IBM MQ on Linux and Windows
- IBM MQ on z/OS
- IBM MQ in containers (using the `icr.io/ibm-messaging/mq` image)
- IBM MQ as a Service on IBM Cloud

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

## Configuring the endpoint

To use IBM MQ as the underlying transport:

```csharp
var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

var transport = new IbmMqTransport(options =>
{
    options.Host = "mq-server.example.com";
    options.Port = 1414;
    options.Channel = "DEV.APP.SVRCONN";
    options.QueueManagerName = "QM1";
    options.User = "app";
    options.Password = "passw0rd";
});

endpointConfiguration.UseTransport(transport);
```

See [connection settings](connection-settings.md) for all available connection and configuration options.

## Advantages and disadvantages

### Advantages

- Enterprise-grade messaging platform with decades of proven reliability in mission-critical systems.
- Native publish-subscribe mechanism; does not require NServiceBus persistence for storing event subscriptions.
- Supports [atomic sends with receive](/transports/transactions.md), ensuring send and receive operations commit or roll back together.
- Integrates with mainframe and legacy systems that already use IBM MQ, bridging .NET applications with z/OS, IBM i, and other platforms.
- Built-in high availability via multi-instance queue managers and connection name lists.
- Supports SSL/TLS encryption and certificate-based authentication.
- Supports the [competing consumer](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) pattern out of the box for horizontal scaling.

### Disadvantages

- Requires an IBM MQ license; IBM MQ is a commercial product.
- Queue and topic names are limited to 48 characters, which can require [custom name sanitization](connection-settings.md#resource-name-sanitization).
- Does not support native delayed delivery; requires an external timeout storage mechanism.
- Does not support `TransactionScope` mode (distributed transactions).
- Fewer .NET community resources compared to RabbitMQ or cloud-native alternatives.
- Queue manager administration requires specialized IBM MQ knowledge.
