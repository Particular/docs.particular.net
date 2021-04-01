---
title: Azure Storage Queues Native Publish Subscribe
summary: Describes the native publish/subscribe implementation in the Azure Storage Queues transport
reviewed: 2021-04-01
component: ASQ
versions: '[10,)'
---

Azure Storage Queues implements the publish/subscribe (pub/sub) pattern. In version 9 and below, this feature relies on message-driven pub/sub which requires a separate persistence for storage of subscription information. In version 10 and above, the transport handles subscription information natively and a separate persistence is not required.

The transport creates a dedicated subscription routing table shared by all endpoints, which holds subscription information for each event type.

When an endpoint subscribes to an event, an entry is created in the subscription routing table.

When an endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints. Polymorphism is implemented on the publisher side by dissecting the published typed into its hierarchy and then reading all subscribers on those topics into a distinct list of queue addresses. By doing so, automatic deduplication on the hierarchy is guaranteed.

![image](native-pubsub-01.png)

All multi-cast operations are transformed into unicast operations to ensure multi-storage account support still works.

### Multi storage account support

When the transport supported message-driven pub/sub in earlier versions, the multi-storage account support relied on the publisher information and the fact that the subscription messages were sent to the publisher queue. With the native pub/sub implementation, this approach no longer works.

Multi-storage account support relies on an agreement of connection string information between publisher and subscriber. To keep this connection information secure, the transport leverages the account alias metadata.

Using this alias metadata both subscriber and publisher can derive the connection string instead of storing it. This allows the subscriber to insert a subscription record into the subscription table on the publisher's storage account with a queue address using only the subscriber alias.

Since publishers use unicast operations to send events to the subscriber queue directly, they require the subscriber alias and the connection to be configured on the publisher as well.

When the publisher dispatches the unicast operations, it queries for the interested subscribers, which then returns local non-aliased subscribers together with remote (i.e. aliased) subscribers.

![image](native-pubsub-02.png)


The following endpoint configuration is required to make sure that `Subscriber1` can subscribe to `OrderAccepted`, and that `Publisher1` can send the message. The publisher configuration:

snippet: storage_account_routing_registered_subscriber

The subscriber configuration:

snippet: storage_account_routing_registered_publisher
