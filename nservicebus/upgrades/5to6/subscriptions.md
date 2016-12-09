---
title: Subscription changes in Version 6
reviewed: 2016-10-26
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Storage

`ISubscriptionStorage` has been split into two interfaces, `ISubscriptionStorage` and `IInitializableSubscriptionStorage`, to properly separate those storage concerns. `ISubscriptionStorage` must be implemented to have a fully functional subscription infrastructure. `IInitializableSubscriptionStorage` is only necessary when the subscription storage needs to be initialized.

`ISubscriptionStorage` implements the concern of storage, retrieval, and removal for subscriptions, which is executed inside the context of a pipeline. Furthermore, `ISubscriptionStorage` introduced a new parameter `SubscriptionStorageOptions`. This parameter allows access to the pipeline context. This enables subscription storages to manipulate everything that exists in the context during message pipeline execution.


## Auto subscribing plain messages

The option to automatically subscribe to plain messages was removed, as message subscription should be based on events. Although not recommended, this can be overridden by [manually subscribing](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#manually-subscribing-to-a-message) to other message types.


## SubscriptionEventArgs has been deprecated

Version 5 introduced an undocumented way to get the list of subscribers when publishing a message on the transports using [persistence based pub/sub](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based-message-driven). This is no longer available; [contact support](https://particular.net/support) should this information be required in Version 6.


## AutoSubscribe

The configuration option `DoNotRequireExplicitRouting()` has been obsoleted since transports with support for centralized pubsub will always auto subscribe all events without requiring explicit routing. Transports with message driven pubsub (like [MSMQ](/nservicebus/msmq/), [Sql Server](/nservicebus/sqlserver/) and [AzureStorageQueues](/nservicebus/azure-storage-queues/)) will not subscribe properly if there is no routing specified. If previously this was used it can now safely remove it.

AutoSubscription happens during the startup phase of the bus. Previous versions of NServiceBus tried to subscribe multiple times on a background thread until the subscription either succeeded or failed. When the subscription failed, an error entry was written to the log file. This version of NServiceBus changes that behavior for transports with message driven pub-sub. The subscription is tried asynchronously on the startup thread. In the case when a subscriber starts and the publisher has never created its queues, the subscriber endpoint will not start and the caller will receive a `QueueNotFoundException` indicating what went wrong.


## MSMQ Subscription Authorization

[MSMQ Subscription Authorization](/nservicebus/msmq/subscription-authorisation.md) is now done by the `SubscriptionAuthorizer` delegate at configuration time and not the `IAuthorizeSubscriptions` interface.

snippet: 5to6-MsmqSubscriptionAuthorizer
