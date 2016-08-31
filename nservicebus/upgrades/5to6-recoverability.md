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


## Legacy .Retries queue

The `.Retries` Satellite queue is no longer necessary when running Version 6 of NServiceBus.  However, when starting a Version 6 endpoint, unless explicitly configured, a dedicated receiver that watches the .retries queue will still be started. This default configuration is mainly for a one-time scenario necessary to prevent message loss when upgrading from Version 5 to Version 6. 


### When Upgrading from Versions 5 and below

In NServiceBus Versions 5 and below, the [Delayed Retries](/nservicebus/recoverability/#delayed-retries) of NServiceBus used the `[endpoint_name].Retries` queue to durably store messages before persisting them for retries.  When upgrading a Version 5 endpoint to Version 6, during the process of stopping the endpoint, there is a possibility that the .retries queue may contain some of these messages that were meant to be delayed and retried. Therefore to prevent message loss, when starting up a Version 6 endpoint, the .retries satellite receiver runs to serve a one-time purpose of forwarding those messages from the `.retries` queue to the endpoint's main queue to be retried appropriately. 

Once the upgrade is done the receiver can be safely [disabled](/nservicebus/recoverability/configure-delayed-retries.md#legacy-retries-message-receiver) and the `.Retries` queue can be safely deleted.


### When Creating New Endpoints using Version 6

In Version 6, the only reason that the `.retries` queue exists is so that Version 5 endpoints can be migrated to Version 6 without any message loss. Any endpoints that are written purely using Version 6, can safely use the following configuration API to disable the satellite from even starting.