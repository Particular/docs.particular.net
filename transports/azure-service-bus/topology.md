---
title: Topology
component: ASBS
reviewed: 2022-03-11
---

Messaging topology is a specific arrangement of the messaging entities, such as queues, topics, subscriptions, and rules.

Azure Service Bus transport operates on a topology created on the broker. The topology handles exchanging messages between endpoints, by creating and configuring Azure Service Bus entities.

The built-in topology, also known as `ForwardingTopology`, was introduced to take advantage of the broker nature of the Azure Service Bus and to leverage its native capabilities.

The topology creates a single input queue per endpoint and implements a [publish-subscribe](/nservicebus/messaging/publish-subscribe/) mechanism, with all publishers using a single topic.


Subscriptions are created under the topic, with one subscription entity per subscribing endpoint. Each subscription contains multiple rules; there's one rule per event type that the subscribing endpoint is interested in. This enables a complete decoupling between publishers and subscribers. All messages received by subscription are forwarded to the input queue of the subscriber.

![ForwardingTopology](forwarding-topology.png "width=500")


#### Quotas and limitations

The `ForwardingTopology` supports up to 2,000 endpoints with up to 2,000 events in total. Since multiple rules per subscription are used, topics cannot be partitioned.

#### Subscription rule matching

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%(full class name)%'
```

The [SQL rules added on the subscriber](https://docs.microsoft.com/en-us/azure/service-bus-messaging/topic-filters) side will match the [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) to the fully qualified class name, including `%` at the beginning and `%` at the end. `%` stands for [Any string of zero or more characters](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-sql-filter#pattern).

As an example, a subscriber interested in the event `Shipping.OrderAccepted` will add a rule to the subscription with the following content:

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAccepted%'
```

The subscription rules uses `%` both at the beginning and at the end of the ` LIKE` clause to support the following scenarios:

- Interface-based inheritance
- Evolution of the message contract

##### Interface-based inheritance

A published message type might have in its hierarchy multiple valid interfaces that represent a message type. As an example

```csharp
namespace Shipping;

interface IOrderAccepted : IEvent { }
interface IOrderStatusChanged : IEvent { }

class OrderAccepted : IOrderAccepted, IOrderStatusChanged { }
class OrderDeclined : IOrderAccepted, IOrderStatusChanged { }
```

in such a scenario, the [EnclosedMessageTypes headers](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) when publishing the `OrderAccepted` event looks like the following:

```
Shipping.OrderAccepted, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderAccepted, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderStatusChanged, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ
```

For a handler `class OrderAcceptedHandler : IHandleMessages<OrderAccepted>` the subscription will look like

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAccepted%'
```

making sure the condition matches the above shown [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes).

Should the subscriber be only interested in the interface `IOrderStatusChanged` by declaring a handler `class OrderStatusChanged : IHandleMessages<IOrderStatusChanged>` the corresponding subscription will look like


```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.IOrderStatusChanged%'
```

and match any [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) that contains `Shipping.IOrderStatusChanged` regardless of the position in the header. When a publisher starts publishing `Shipping.OrderDeclined` the event will automatically be routed into the subscriber's input queue without any topology changes.

##### Evolution of the message contract

As mentioned in [versioning of shared contracts](/nservicebus/messaging/sharing-contracts.md#versioning) and also shown in the examples above, NServiceBus uses the fully qualified assembly name in the message header. [Evolving the message contract](/nservicebus/messaging/evolving-contracts.md) encourages creating entirely new contract types and then adding a version number to the original name. For example, when evolving `Shipping.OrderAccepted` a new contract would be created by the publisher called `Shipping.OrderAcceptedV2`. When the publisher starts publishing `Shipping.OrderAcceptedV2` the enclosed message type would look like the following

```
Shipping.OrderAcceptedV2, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderAccepted, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderStatusChanged, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ
```

any existing subscriber being subscribed to

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAccepted%'
```

will still receive those events automatically due to the base name of `Shipping.OrderAcceptedV2` being `Shipping.OrderAccepted`. New subscribers subscribing to the updated contract will subscribe with

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAcceptedV2%'
```

and contain the closer match of the newly created contract.

#### Topology highlights

|                                             |                     |
|---------------------------------------------|---------------------|
| Decoupled Publishers / Subscribers          |  yes                |
| Polymorphic events support                  |  yes                |
| Event overflow protection                   |  yes                |
| Subscriber auto-scaling based on queue size |  yes                |
| Reduced number of connections to the broker |  yes                |
