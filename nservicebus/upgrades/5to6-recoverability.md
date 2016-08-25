---
title: Upgrade Version 5 to 6 - Recoverability
reviewed: 2016-08-29
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/5to6
- nservicebus/recoverability
---

## Renaming

First Level Retries (FLR) has been renamed to [Immediate Retries](/nservicebus/recoverability/#immediate-retries).

Second Level Retries (SLR) have been renamed to [Delayed Retries](/nservicebus/recoverability/#delayed-retries).


## Code First API

Recoverability can now be configured using a code first API:

snippet: 5to6-RecoverabilityCodeFirstApi

The api enables setting all automatic retry parameters that were previously available only with configuration source approach.


## Enabling and disabling Immediate Retries Delayed Retries

`FirstLevelRetries` and `SecondLevelRetries` are no longer features. As a result `configuration.DisableFeature<FirstLevelRetries>()` and `configuration.DisableFeature<SecondLevelRetries>()` cannot be used to disable them. Equivalent behavior can be achieved through code API by setting Immediate Retries and Delayed Retries to `0`:

snippet: 5to6-RecoverabilityDisableRetries


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

## Legacy `.Retries` queue

In NServiceBus 5 all messages scheduled for delayed retry where forwarded to `.Retries` queue. In version 6 this queue is no longer used, however it is possible that after upgrading from version 5 the queue will contain unprocessed messages.

To prevent message loss, version 6 runs a dedicated receiver that processes any messages left in `.Retries` queue. The receiver is enabled by default. It is safe to disable it and delete `.Retries` queue if the endpoint did not use Second Level Retries or after upgrade when the queue is empty.   

The `.Retries` queue receiver can be disabled via code API.

snippet: 5to6-DisableLegacyRetriesSatellite  