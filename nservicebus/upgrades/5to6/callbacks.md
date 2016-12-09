---
title: Callback Changes in Version 6
reviewed: 2016-10-26
component: Callbacks
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

The synchronous request-response feature, also known as [Callbacks](/nservicebus/messaging/callbacks.md)., has been moved from the NServiceBus core to the separate Nuget package [NServiceBus.Callbacks](https://www.nuget.org/packages/NServiceBus.Callbacks/). That package must be used in order to use the callback functionality in Version 6.

The API was also modified. Version 6 API is asynchronous by default and allows to easily access the response message. It is no longer possible to use callbacks inside handlers or sagas, because extension methods are only available on the message session. The differences in the API are fully covered in [Callbacks](/nservicebus/messaging/callbacks.md).

The `NServiceBus.Callbacks` package has to be referenced only by the requesting endpoint. The responding endpoint only need `NServiceBus.Callbacks` if it replies  with `int` or `enum` types.

snippet: 5to6-Callbacks

In Version 6 the callback handling on [broker transports](/nservicebus/transports/#types-of-transports-broker-transports) is no longer based on the machine host name. In order to continue using callbacks when using a broker transport, the endpoint needs to be uniquely addressable:

snippet: 5to6-Callbacks-InstanceId

NOTE: This ID should never be hardcoded, e.g. it can be read from the configuration file or from the environment (e.g. role ID in Azure), so that it can be changed without code changes and redeployment.