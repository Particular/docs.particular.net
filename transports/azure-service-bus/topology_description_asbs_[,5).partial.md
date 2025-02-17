The built-in topology, also known as `ForwardingTopology`, was introduced to leverage Azure Service Bus' broker model and its native capabilities.

The topology creates a single input queue per endpoint and implements a [publish-subscribe](/nservicebus/messaging/publish-subscribe/) mechanism, with all publishers using a single topic.

Subscriptions are created under the topic, with one subscription entity per subscribing endpoint. Each subscription contains multiple rules; there's one rule per event type that the subscribing endpoint is interested in. This enables a complete decoupling between publishers and subscribers. All messages received by subscription are forwarded to the subscriber's input queue.

![ForwardingTopology](forwarding-topology.png "width=500")

#### Quotas and limitations

The `ForwardingTopology` is subject to [quotas](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas) of Azure Service Bus. A single topic supports up to 2000 subscriptions and 2000 filters/rules. For every logical endpoint, one subscription is created that contains one rule per event type subscribed, as shown on the topology overview.

For example, in larger systems, there may be dozens of event types for various business components to interact. Considering a system with 100 different event types, with every endpoint subscribing to 25% of the events, an endpoint would have to manage at least 25 rules. This means the `ForwardingTopology` can handle up to 80 different logical endpoints until it reaches its scaling limits. The more events there are and the more an endpoint is interested in subscribing to them, the fewer endpoints can be added to the system.

It is possible to create a hierarchy of topics by separating the publish topic from the subscribe topic. Due to the added complexity of the hierarchy and the increased number of operations, it is encouraged to start with a single-topic topology and reevaluate options when approximately 50% of the capacity is reached.

For example, an existing system using `bundle-1` as the default single topic topology can add new endpoints as subscribers under `bundle-2` but continue publishing all their events to `bundle-1`. This ensures non-interrupted communication between the endpoints while making it possible to scale beyond the limits of a single topic. A newly added endpoint would need to configure its topology as follows:

snippet: custom-topology-hierarchy-bundle

This creates a new `bundle-2` topic, a subscription called `forward-bundle-2` on the `bundle-1`topic, which automatically forwards all events published to `bundle-1` to `bundle-2`. The new endpoint creates its subscription under `bundle-2` as shown in the picture below:

![Topology Hierarchy](forwarding-topology-hierarchy.svg "width=500")

> [!NOTE]
> While it is technically possible to create even deeper hierarchies (e.g. attaching a `bundle-3` to `bundle-2`), it is strongly discouraged to do so due to the complexity and limitations of the number of hops a message can be routed through. The maximum allowed number of hops in Azure Service Bus is four and needs to be considered when creating a topology hierarchy. In addition, introducing more forwarding hops also increases the number of operations used, affecting pricing or memory/CPU used depending on the selected Azure Service Bus tier.

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

A published message type can have multiple valid interfaces in its hierarchy representing a message type. For example:

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

This matches any [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) that contains `Shipping.IOrderStatusChanged` regardless of the position in the header. When a publisher starts publishing `Shipping.OrderDeclined` the event is automatically routed to the subscriber's input queue without any topology changes.

##### Evolution of the message contract

As mentioned in [versioning of shared contracts](/nservicebus/messaging/sharing-contracts.md#versioning) and also shown in the examples above, NServiceBus uses the fully-qualified assembly name in the message header. [Evolving the message contract](/nservicebus/messaging/evolving-contracts.md) encourages creating entirely new contract types and then adding a version number to the original name. For example, when evolving `Shipping.OrderAccepted`, the publisher would create a new contract called `Shipping.OrderAcceptedV2`. When the publisher publishes `Shipping.OrderAcceptedV2` events, the enclosed message type would look as follows:

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

The "single topic" design was chosen over a "topic per event type" design due to:

- Support for polymorphic events
- Stronger decoupling of the publishers and subscribers
- Simpler hierarchy of topics, subscriptions, and rules
- Simpler management and mapping from events to parts of the topology

The need to support polymorphism was the most substantial reason to implement a "single topic" design.

For simple events or when support for polymorphism is not needed, it is possible to map the contract type to the topic since the relationship is 1:1. Such topologies are mostly built without automatically forwarding messages in subscriptions to the destination queues. The subscription acts as a virtual queue. In cases when subscribers are offline or slow for some time, the quota of the topic may be reached, which can cause all publish operations to the topic to fail. Subscriptions in non-forwarding mode share the topic quota.

Enabling forwarding on the subscription, resolves the problem. The benefit of the event-per-topic approach, is that the filter rules can be the default catch-all rule expressed as `1 = 1`. Those are relatively straightforward to evaluate at runtime by the broker and do not impose high-performance impacts. The client-to-broker interaction rises in complexity as soon as polymorphism is required. The subscriber needs to be able to figure out all the possible contracts of the event and create multiple subscriptions on multiple topics based on the contracts exposed. But in some cases, the subscriber doesn't have access to the whole event contract the publisher publishes. In such cases, complex manual mapping is required to determine the publisher-subscriber relationship. Such a design imposes more coupling between the publisher and the subscriber, serving the opposite of what publish/subscribe is intended to solve.

On the other hand, the publisher needs to figure out to what possible topics the events should be published and then publish by issuing multiple operations against multiple entities initiated by the client. These operations are impacted by the latency between the client and the broker, might fail, and must be retried in case of transient errors. Once multiple subscriptions on multiple topics must be managed, duplicates will likely occur because the broker duplicates the original message per subscription. In the case of polymorphism, the publisher might be publishing the same event to multiple topics, which naturally leads to duplicates.

The complexity of managing all the topics, subscriptions and rules can increase quickly. With the single topic design, only a common topic bundle and a subscription per endpoint containing the rules that match the enclosed message type headers are required. Polymorphism and de-duplication support is built into the design. The downside is that more advanced SQL filters for the rules can impact the namespace performance when the broker evaluates rules, which could impose a performance penalty in extremely high throughput scenarios.

#### Topology highlights

|                                             |                     |
|---------------------------------------------|---------------------|
| Decoupled Publishers / Subscribers          |  yes                |
| Polymorphic events support                  |  yes                |
| Event overflow protection                   |  yes                |
| Subscriber auto-scaling based on queue size |  yes                |
| Reduced number of connections to the broker |  yes                |
