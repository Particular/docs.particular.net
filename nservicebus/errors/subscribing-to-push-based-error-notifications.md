---
title: Push-based error notifications
summary: Subscribing to push-based error notifications using reactive extensions
tags: []
redirects:
 - nservicebus/subscribing-to-push-based-error-notifications
---

From version 5.1, we have exposed push-based error notifications using the [`IObservable` interface](https://msdn.microsoft.com/en-us/library/dd990377.aspx).

These new event streams are exposed via the `BusNotifications` class that can be injected via DI.

The following example shows how to be notified every time a message is about to be sent to the configured error queue so that an email is sent notifying interested parties.

snippet:SubscribeToErrorsNotifications

For more information about Reactive Extensions, see https://msdn.microsoft.com/en-au/data/gg577609.aspx

{{DANGER:
It is important to ensure all subscriptions are handled in another thread, the example code provided above does that by using `ObserveOn(Scheduler.Default)`. Failure to do so may impact the overall system performance. See [schedulers](https://msdn.microsoft.com/en-us/library/hh242963.aspx) for more information.
}}

{{DANGER:
It is also important to ensure no longer required subscriptions are disposed, this is done by keeping a reference to the `IDisposable` object returned from calling the `Subscribe` method on the `IObservable` and calling its `Dispose` method. See https://msdn.microsoft.com/en-us/library/dd782981.aspx for more information.
}}
