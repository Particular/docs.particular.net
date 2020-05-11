---
title: Subscription Changes in NServiceBus Version 6
reviewed: 2020-05-11
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Storage

`ISubscriptionStorage` has been split into two interfaces, `ISubscriptionStorage` and `IInitializableSubscriptionStorage`, to properly separate subscription storage concerns. `ISubscriptionStorage` must be implemented to have a fully functional subscription infrastructure. `IInitializableSubscriptionStorage` is necessary only when the subscription storage needs to be initialized.

`ISubscriptionStorage` implements the concern of storage, retrieval, and removal for subscriptions, which is executed inside the context of a pipeline. Furthermore, `ISubscriptionStorage` introduced a new parameter: `SubscriptionStorageOptions`. This parameter allows access to the pipeline context which enables subscription storages to manipulate everything that exists in the context during message pipeline execution.


## Automatically subscribing plain messages

The option to automatically subscribe to plain messages was removed, as message subscription should be based on events. Although not recommended, this can be overridden by [manually subscribing](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#manually-subscribing-to-a-message) to other message types.


## SubscriptionEventArgs has been deprecated

NServiceBus version 5 introduced an undocumented way to get the list of subscribers when publishing a message on the transports using [persistence based pub/sub](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based). This is no longer available; [contact support](https://particular.net/support) should this information be required in version 6.


## Automatic subscription

The configuration option `DoNotRequireExplicitRouting()` is obsolete since transports with support for centralized pub/sub will always auto subscribe all events without requiring explicit routing. Transports with message-driven pub/sub (like [MSMQ](/transports/msmq/), [SQL Server](/transports/sql/), and [Azure Storage Queues](/transports/azure-storage-queues/)) will not subscribe properly if there is no routing specified. Any code that uses this option can now be safely removed.

Automatic subscription happens during the startup phase of the bus. Previous versions of NServiceBus tried to subscribe multiple times on a background thread until the subscription either succeeded or failed. When the subscription failed, an error entry was written to the log file. NServiceBus version 6 changes that behavior for transports with message-driven pub/sub. Subscription is tried asynchronously on the startup thread. In the case when a subscriber starts and the publisher has never created its queues, the subscriber endpoint will not start and the caller will receive a `QueueNotFoundException` indicating what went wrong.


## MSMQ subscription authorization

[MSMQ subscription authorization](/transports/msmq/subscription-authorisation.md) is now done by the `SubscriptionAuthorizer` delegate at configuration time and not by the `IAuthorizeSubscriptions` interface.

snippet: 5to6-MsmqSubscriptionAuthorizer
