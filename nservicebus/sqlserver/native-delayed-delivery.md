---
title: SQL Server Native Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQL Server transport
reviewed: 2017-06-05
component: SqlServer
versions: '[3,)'
---

In Versions 3.1 and above, the SQL Server transport no longer relies on the [timeout manager](/nservicebus/messaging/timeout-manager.md) to implement [delayed delivery](/nservicebus/messaging/delayed-delivery.md). Instead, the transport creates infrastructure which can delay messages using native SQL Server transport features.


## How it works

The transport creates an additional queue that stores delayed messages. By default it's called `...`, the suffix can be modified using the following setting:

TODO: code snippet with a setting `MessageStoreTableSuffix`

The timeouts poller checks for expired messages every X seconds, as specified using XYZ setting:

TODO: code snippet with a setting `MessageStoreProcessingResolution`

The poller picks and dispatches messages in batches of ....


## Backwards compatibility

When upgrading to a version of the transport that supports delayed delivery natively, it is safe to operate a combination of native-delay and non-native-delay endpoints at the same time. Native endpoints can send delayed messages to endpoints that are not yet aware of the native delay infrastructure. Native endpoints can continue to receive delayed messages from non-native endpoints as well.


### Disabling the timeout manager

To assist with the upgrade process, the timeout manager is still enabled by default, so any delayed messages already stored in the endpoint's persistence database before the upgrade will be sent when their timeouts expire. Any delayed messages sent after the upgrade will be sent through the delay infrastructure even though the timeout manager is enabled.

Once an endpoint has no more delayed messages in its persistence database, there is no more need for the timeout manager. It can be disabled by calling:

TODO: code snippet with a setting disabling timeout manager

At this point, the `.Timeouts` and `.TimeoutsDispatcher` exchanges and queues for the endpoint can be deleted from the broker. In addition, the endpoint no longer requires timeout persistence, so those storage entities can be removed from the persistence database as well.

NOTE: Newly created endpoints that have never been deployed without native delayed delivery should also set this value to prevent the timeout manager from running.
