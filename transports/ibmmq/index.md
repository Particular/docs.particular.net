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

The transport requires IBM MQ 9.0 or later. It uses the managed .NET client library (`IBMMQDotnetClient`) which communicates with queue managers via the client connection (SVRCONN) channel.

The transport has been tested with:

- IBM MQ on Linux and Windows
- IBM MQ on z/OS
- IBM MQ in containers (using the `icr.io/ibm-messaging/mq` image)
- IBM MQ as a Service on IBM Cloud

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions               |None, ReceiveOnly, SendsAtomicWithReceive
|Pub/Sub                    |Native (via IBM MQ topics and durable subscriptions)
|Timeouts                   |Not natively supported
|Large message bodies       |Up to 100 MB (configurable, must match queue manager MAXMSGL)
|Scale-out                  |Competing consumer
|Scripted Deployment        |Supported via [CLI tool](operations-scripting.md) and IBM MQ admin commands
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
- Native publish-subscribe mechanism via IBM MQ topics and durable subscriptions; does not require NServiceBus persistence for storing event subscriptions.
- Supports atomic sends with receive using IBM MQ syncpoint, ensuring send and receive operations commit or roll back together.
- Integrates with mainframe and legacy systems that already use IBM MQ, bridging distributed .NET applications with z/OS, IBM i, and other platforms.
- Built-in high availability via multi-instance queue managers and connection name lists.
- Supports SSL/TLS encryption and certificate-based authentication for secure communication.
- Supports the [competing consumer](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) pattern out of the box for horizontal scaling.

### Disadvantages

- Requires an IBM MQ license; IBM MQ is a commercial product with per-VPC or per-core licensing.
- IBM MQ queue and topic names are limited to 48 characters, which can require custom name sanitization for longer endpoint or event type names.
- Does not support native delayed delivery; delayed delivery requires an external timeout storage mechanism.
- Does not support `TransactionScope` mode (distributed transactions).
- Fewer .NET-focused community resources compared to RabbitMQ or cloud-native alternatives.
- Queue manager administration requires specialized IBM MQ knowledge (runmqsc, MQ Explorer, or equivalent tooling).

## Queue naming constraints

IBM MQ imposes strict naming rules on queues and topics:

- Maximum 48 characters
- Allowed characters: `A-Z`, `a-z`, `0-9`, `.`, `_`
- No hyphens, spaces, or other special characters

If endpoint or event type names exceed these constraints, configure a `ResourceNameSanitizer` to transform names:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.ResourceNameSanitizer = name =>
    {
        // Replace invalid characters and truncate
        var sanitized = name.Replace("-", ".").Replace("/", ".");
        return sanitized.Length > 48 ? sanitized[..48] : sanitized;
    };
});
```

> [!WARNING]
> Ensure the sanitizer produces deterministic and unique names. Two different input names mapping to the same sanitized name will cause messages to be delivered to the wrong endpoint.

## Message persistence

By default, all messages are sent as persistent (`MQPER_PERSISTENT`), meaning they survive queue manager restarts. To send non-persistent messages for higher throughput at the expense of durability, mark messages with the `NonDurableMessage` header.

> [!CAUTION]
> Non-persistent messages are lost if the queue manager restarts before they are consumed.
