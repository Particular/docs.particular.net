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

The [SQL rules added on the subscriber](https://docs.microsoft.com/en-us/azure/service-bus-messaging/topic-filters) side will match the [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) to the fully qualified class name, including `%` at the beginning and `%` at the end. In this case, `%` follows standard SQL syntax and stands for [any string of zero or more characters](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-sql-filter#pattern).

For example, a subscriber interested in the event `Shipping.OrderAccepted` will add the following rule to the subscription:

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAccepted%'
```

The subscription rules uses `%` both at the beginning and at the end of the ` LIKE` clause to support the following scenarios:

- Interface-based inheritance
- Evolution of the message contract

##### Interface-based inheritance

A published message type might have in its hierarchy multiple valid interfaces that represent a message type. For example:

```csharp
namespace Shipping;

interface IOrderAccepted : IEvent { }
interface IOrderStatusChanged : IEvent { }

class OrderAccepted : IOrderAccepted, IOrderStatusChanged { }
class OrderDeclined : IOrderAccepted, IOrderStatusChanged { }
```

In this scenario, the [EnclosedMessageTypes headers](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) looks like the following when publishing the `OrderAccepted` event:

```
Shipping.OrderAccepted, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderAccepted, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderStatusChanged, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ
```

For a handler `class OrderAcceptedHandler : IHandleMessages<OrderAccepted>` the subscription will look like:

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAccepted%'
```

This ensures that the condition matches the [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) shown previously.

If the subscriber is interested only in the interface `IOrderStatusChanged`, it will declare a handler `class OrderStatusChanged : IHandleMessages<IOrderStatusChanged>` and the corresponding subscription will look like:


```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.IOrderStatusChanged%'
```

This matches any [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) that contains `Shipping.IOrderStatusChanged` regardless of the position in the header. When a publisher starts publishing `Shipping.OrderDeclined` the event will automatically be routed into the subscriber's input queue without any topology changes.

##### Evolution of the message contract

As mentioned in [versioning of shared contracts](/nservicebus/messaging/sharing-contracts.md#versioning) and also shown in the examples above, NServiceBus uses the fully-qualified assembly name in the message header. [Evolving the message contract](/nservicebus/messaging/evolving-contracts.md) encourages creating entirely new contract types and then adding a version number to the original name. For example, when evolving `Shipping.OrderAccepted` a new contract would be created by the publisher called `Shipping.OrderAcceptedV2`. When the publisher publishes `Shipping.OrderAcceptedV2` events, the enclosed message type would look like the following:

```
Shipping.OrderAcceptedV2, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderAccepted, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ;Shipping.IOrderStatusChanged, Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=XYZ
```

Any existing subscriber that subscribes to:

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAccepted%'
```

will still receive those events automatically since the base name of `Shipping.OrderAcceptedV2` is `Shipping.OrderAccepted`. New subscribers subscribing to the updated contract can subscribe with:

```sql
[NServiceBus.EnclosedMessageTypes] LIKE '%Shipping.OrderAcceptedV2%'
```

which matches the newly created contract more closely.

#### Single topic design

The "single topic" over a "topic per event type" design was chosen primarily due to:

- Support for polymorphic events
- Stronger decoupling of the publishers and subscribers
- Simpler hierarchy of topic, subscriptions, and rules
- Simpler management and mapping from events to parts of the topology

The need to support polymorphism was the most substantial reason to implement a "single topic" design.

With simple events and no need to support polymorphism, it is possible to map the contract type to the topic since the relationship is 1:1. Such topologies are mostly built without automatically forwarding messages in subscriptions to the destination queues. The subscription acts in such a scenario as a virtual queue. In cases when subscribers are offline or slow for some time, the quota of the topic might be reached, which can cause all publish operations to the topic to fail. Subscriptions in non-forwarding mode share the topic quota.

Once forwarding is enabled on that subscription, that problem will go away. The benefit of the simple event to topic mapping is that the filter rules can be the default catch-all rule expressed as `1 = 1`. Those are relatively straightforward to evaluate at runtime by the broker and do not impose high-performance impacts. The client-to-broker interaction gets more complex as soon as the need for polymorphism comes into play. The subscriber needs to be able to figure out all the possible contracts of the event and create multiple subscriptions on multiple topics based on the contracts exposed. But in some cases, the subscriber doesn't have access to the whole event contract the publisher publishes. Then complex manual mapping would be required to determine the publisher and subscriber relationship. Such a design imposes more coupling between the publisher and the subscriber, serving the opposite of what publish/subscribe is intended to solve.

On the other hand, the publisher needs to figure out to what possible topics the events should be published and then publish by issuing multiple operations against multiple entities initiated by the client. These operations are impacted by the latency between the client and the broker, might fail, and must be retried in case of transient errors.
Once multiple subscriptions on multiple topics must be managed, duplicates will likely occur because the broker duplicates the original message per subscription. In the case of polymorphism, the publisher might be publishing the same event to multiple topics, which naturally leads to duplicates.

The complexity of managing all the topics, subscriptions and rules would get out of hand quickly. With the single topic design, only a common topic bundle and a subscription per endpoint that contains the rules that match the enclosed message type headers are required. Polymorphism and de-duplication support is built into the design. The downside is that more advanced SQL filters for the rules can impact the namespace performance when the broker evaluates rules, which could impose a performance penalty in extremely high throughput scenarios.

#### Topology highlights

|                                             |                     |
|---------------------------------------------|---------------------|
| Decoupled Publishers / Subscribers          |  yes                |
| Polymorphic events support                  |  yes                |
| Event overflow protection                   |  yes                |
| Subscriber auto-scaling based on queue size |  yes                |
| Reduced number of connections to the broker |  yes                |
