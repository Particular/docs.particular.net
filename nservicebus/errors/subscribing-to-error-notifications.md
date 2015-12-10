---
title: Error notifications
summary: Subscribing to error notifications
tags: []
redirects:
 - nservicebus/subscribing-to-push-based-error-notifications
 - nservicebus/errors/subscribing-to-push-based-error-notifications
---

Error notifications are available for several events.

 * When a first level retie occurs.
 * When a second level retry occurs.
 * When a message fails retries and is forwarded to the error queue.

This API was added in version 5.1.

These event are exposed via the `BusNotifications` class that can be injected via DI.

The following example shows how to be notified every time a message is about to be sent to the configured error queue so that an email is sent notifying interested parties.

snippet:SubscribeToErrorsNotifications


## Performance Impact

Notifications re fired on the same thread as the the executing pipeline. As such, for long running actions, It is important to ensure all subscriptions are handled in another thread


## Reactive Extensions

Between Version 5.1 and before Version 6 the subscription was done via Reactive Extensions.

For more information about Reactive Extensions, see https://msdn.microsoft.com/en-au/data/gg577609.aspx


## Unsubscribing

Since notifications are global for the current endpoint it is also important to ensure no longer required subscriptions removed so as to nor unnecessarily impact performance.

In version 6 and higher this is done by detaching from the event. 

In Version 5.1 and before Version 6 this is done by keeping a reference to the `IDisposable` object returned from calling the `Subscribe` method on the `IObservable` and calling its `Dispose` method. See https://msdn.microsoft.com/en-us/library/dd782981.aspx for more information.