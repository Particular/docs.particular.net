---
title: Azure Storage Queues Native Publish Subscribe
summary: Describes the native publish/subscribe implementation in the Azure Storage Queues transport
reviewed: 2024-04-30
component: ASQ
versions: '[10,)'
---

Azure Storage Queues implements the [publish/subscribe (pub/sub) pattern](/nservicebus/messaging/publish-subscribe/). In version 9 and below, Azure Storage Queues relied on message-driven pub/sub, which requires a separate persistence for storage of subscription information. In version 10 and above, the transport can store subscription information natively and a separate persistence is no longer required.

The transport creates a dedicated subscription routing table that holds subscription information for each event type. The subscription data stored in this table is available to all endpoints. When an endpoint subscribes to an event, an entry is created in the subscription routing table.

When an endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.  Each subscriber is sent the event as a unicast operation.

Polymorphism is implemented on the publisher side by dissecting the published type into its hierarchy and then reading all subscribers on those topics into a distinct list of queue addresses. By doing so, automatic deduplication on the hierarchy is guaranteed.

![image](native-pubsub-01.png)

All multi-cast operations are transformed into unicast operations to ensure multi-storage account support still works.

### Multi storage account support

When the transport used message-driven pub/sub in earlier versions, the [multi-storage account support](multi-storageaccount-support.md) relied on the publisher information and that the subscription control messages were sent to the publisher queue ([see Message-driven pub/sub](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based)). With the native pub/sub implementation, this approach no longer works.

Multi-storage account support relies on an agreement of connection string information between publisher and subscriber. To keep this connection information secure, the transport leverages the account alias metadata.

Using this alias metadata both subscriber and publisher can derive the connection string instead of storing it. This allows the subscriber to insert a subscription record into the subscription table on the publisher's storage account with a queue address using only the subscriber alias.

Since publishers use unicast operations to send events to the subscriber queue directly, they require the subscriber alias and the connection to be configured on the publisher as well.

When the publisher dispatches the unicast operations, it queries for the interested subscribers, and then returns local non-aliased subscribers together with remote (i.e. aliased) subscribers.

![image](native-pubsub-02.png)


The endpoint configuration has additional options for registering subscribers and publishers:

snippet: AzureStorageQueuesAddingAdditionalAccounts
