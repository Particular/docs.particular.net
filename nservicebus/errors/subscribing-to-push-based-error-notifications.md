---
title: Push-based error notifications
summary: Subscribing to push-based error notifications using reactive extensions
tags: []
redirects:
 - nservicebus/subscribing-to-push-based-error-notifications
---

From version 5.1, we have exposed error notifications using the.

These new event streams are exposed via the `BusNotifications` class that can be injected via DI.

The following example shows how to be notified every time a message is about to be sent to the configured error queue so that an email is sent notifying interested parties.

snippet:SubscribeToErrorsNotifications


## Performance Impact

Notifications re fired on the same thread as the the executing pipelien. As such, for long running actions, It is important to ensure all subscriptions are handled in another thread


## Reactive Extensions

Between Version 5.1 and before Version 6 the subscription was done via Reactive Extensions.

For more information about Reactive Extensions, see https://msdn.microsoft.com/en-au/data/gg577609.aspx


## Unsubscribing

Since BusNotifications are global for the current endpoint it is also important to ensure no longer required subscriptions removed so as to nor unnecessarily impact performance.

In version 6 and higher this is done by detaching from the event. 

In Version 5.1 and before Version 6 this is done by keeping a reference to the `IDisposable` object returned from calling the `Subscribe` method on the `IObservable` and calling its `Dispose` method. See https://msdn.microsoft.com/en-us/library/dd782981.aspx for more information.