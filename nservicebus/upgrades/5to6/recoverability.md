---
title: Recoverability changes in Version 6
reviewed: 2016-08-29
component: Core
related:
 - nservicebus/upgrades/5to6
 - nservicebus/recoverability
redirects:
 - nservicebus/upgrades/5to6-recoverability
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Renaming

First Level Retries (FLR) has been renamed to [Immediate Retries](/nservicebus/recoverability/#immediate-retries).

Second Level Retries (SLR) have been renamed to [Delayed Retries](/nservicebus/recoverability/#delayed-retries).


## Code First API

Recoverability can now be configured using a code first API:

snippet: 5to6-RecoverabilityCodeFirstApi

The api enables setting all automatic retry parameters that were previously available only with configuration source approach.


## Disabling Immediate Retries

The `TransportConfig` API is obsolete and no longer used to disable Immediate Retries. Equivalent behavior can be achieved through code API by setting Immediate Retries to `0`:

snippet: 5to6-RecoverabilityDisableImmediateRetries


## Disabling Delayed Retries

The `SecondLevelRetries` Feature is not used to disable Delayed Retries. Equivalent behavior can be achieved through code API by setting Delayed Retries to `0`:

snippet: 5to6-RecoverabilityDisableDelayedRetries


## MaxRetries value for First Level Retries

In NServiceBus 5 `MaxRetries` parameter for First Level Retries defined the number of immediate message deliveries, including initial delivery. As a result when set to `1` resulted in no immediate retries being performed. In NServiceBus 6 the meaning of this parameter has been changed and now it defines the number of immediate retries alone (excluding initial delivery). 

In order to get the same behavior in NServiceBus 6 as in NServiceBus 5 the value configured in version 6 should be one less than in version 5.


## [Custom Retry Policy](/nservicebus/recoverability/custom-recoverability-policy.md)

In NServiceBus 5 custom retry policies provided ability to control Second Level Retries, in NServiceBus 6 custom retry policy concept has been substituted by recoverability policy which enables control over every stage of automatic retries as well as error handling.

Recoverability policy operates on a `RecoverabilityConfig` and `ErrorContext` instead of a `TransportMessage` which was the case for custom retry policy.

Custom recoverability policy can be registered using `CustomPolicy` method:

snippet: 5to6-DelayedRetriesCustomPolicy

Custom recoverability policy doesn't have to fully override the default behavior - for example it can provide custom logic for a specific type of error. In any case the default behavior can be reused by calling `Invoke` method on `DefaultRecoverabilityPolicy` class. 

A following snippet is an example of custom recoverability policy that changes delay value calculated by default policy.

snippet: 5to6-DelayedRetriesCustomPolicyHandler

The configuration passed to the custom recoverability policy contains values configured via the recoverability configuration API.


## TransportConfig

`NServiceBus.Config.TransportConfig` has been deprecated. The same can now be achieved via the code API.

snippet: 5to6-configureImmediateRetriesViaCode

snippet: 5to6-configureImmediateRetriesViaXml


## SecondLevelRetriesConfig

`NServiceBus.Config.SecondLevelRetriesConfig` has been deprecated. The same can now be achieved via the code API.

snippet: 5to6-configureDelayedRetriesViaCode

snippet: 5to6-configureDelayedRetriesViaXml


## Legacy .Retries queue

The `.Retries` Satellite queue is no longer necessary when running Version 6 of NServiceBus.  However, when starting a Version 6 endpoint, unless explicitly configured, a dedicated receiver that watches the .retries queue will still be started. This default configuration is mainly for a one-time scenario necessary to prevent message loss when upgrading from Version 5 to Version 6. 


### When Upgrading from Versions 5 and below

In NServiceBus Versions 5 and below, the [Delayed Retries](/nservicebus/recoverability/#delayed-retries) of NServiceBus used the `[endpoint_name].Retries` queue to durably store messages before persisting them for retries.  When upgrading a Version 5 endpoint to Version 6, during the process of stopping the endpoint, there is a possibility that the .retries queue may contain some of these messages that were meant to be delayed and retried. Therefore to prevent message loss, when starting up a Version 6 endpoint, the .retries satellite receiver runs to serve a one-time purpose of forwarding those messages from the `.retries` queue to the endpoint's main queue to be retried appropriately. 

Once the upgrade is done the receiver can be safely [disabled](/nservicebus/recoverability/configure-delayed-retries.md#custom-retry-policy-legacy-retries-message-receiver) and the `.Retries` queue can be safely deleted.


### When Creating New Endpoints using Version 6

In Version 6, the only reason that the `.retries` queue exists is so that Version 5 endpoints can be migrated to Version 6 without any message loss. Any endpoint that is written using Version 6, can safely use the following configuration API to [disable the satellite](/nservicebus/recoverability/configure-delayed-retries.md#custom-retry-policy-legacy-retries-message-receiver).


## IManageMessageFailures is now obsolete.

The `IManageMessageFailures` interface was the extension point to customize the handling of second level retries before a message failure is forwarded to the error queue.

This same functionality and more can be achieved using the [message processing pipeline](/nservicebus/pipeline/). See also [Customizing error handling with the pipeline](/nservicebus/pipeline/customizing-error-handling.md).


## RepeatedFailuresOverTimeCircuitBreaker has been made internal

If are using `RepeatedFailuresOverTimeCircuitBreaker` instead include [the source code](https://github.com/Particular/NServiceBus/blob/5.2.5/src/NServiceBus.Core/CircuitBreakers/RepeatedFailuresOverTimeCircuitBreaker.cs) in the project.


## [Critical Error Action](/nservicebus/hosting/critical-errors.md)

The API for defining a [Critical Error Action](/nservicebus/hosting/critical-errors.md) has been changed to be a custom delegate.

snippet: 5to6CriticalError


## Notifications

The `BusNotifications` class has been renamed to `Notifications`.

`BusNotifications` previously exposed the available notification hooks as observables implementing `IObservable`. This required implementing the `IObserver` interface or including [Reactive-Extensions](https://msdn.microsoft.com/library/hh242985.aspx) to use this API. In Version 6 the notifications API has been changed for easier usage. It exposes regular events instead of observables. To continue using Reactive-Extensions the events API can be transformed into `IObservable`s like this:

snippet: ConvertEventToObservable

Notification subscriptions can now also be registered at configuration time on the `EndpointConfiguration.Notifications` property. See the [error notifications documentation](/nservicebus/recoverability/subscribing-to-error-notifications.md) for more details and samples.

### Access to exception details

Exception details is now available on the passed `Notifications` parameter and not as `NServiceBus.ExceptionInfo.*` headers on the provided message.

### Delayed delivery error notifications

In Versions 6 and above the `TimeoutManager` does not provide any error notifications. When an error occurs during processing of a deferred message by the `TimeoutManager`, the message will be retried and possibly moved to the error queue. The user will not be notified about these events.

Note that in Versions 5 and below, when the user [subscribes to error notifications](/nservicebus/recoverability/subscribing-to-error-notifications.md) they receive notification in the situation described above.


## Timeout Automatic retries

Previously configuring the number of times a message will be retried by the First Level Retries (FLR) mechanism also determined how many times the `TimeoutManager` attempted to retry dispatching a deferred message in case an exception was thrown. From Version 6, the `TimeoutManager` will attempt the dispatch five times (this number is not configurable anymore). The configuration of the FLR mechanism for non-deferred message dispatch has not been changed.
