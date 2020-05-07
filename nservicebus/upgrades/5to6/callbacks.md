---
title: Callback Changes in NServiceBus Version 6
reviewed: 2020-05-07
component: Callbacks
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

The synchronous request-response feature, also known as [callbacks](/nservicebus/messaging/callbacks.md), has been moved from the NServiceBus core package to its own [NServiceBus.Callbacks NuGet package](https://www.nuget.org/packages/NServiceBus.Callbacks/). This package must be used in order to use the callback functionality in NServiceBus version 6.

The API was also modified. The version 6 API is asynchronous by default and allows access to the response message. It is no longer possible to use callbacks inside handlers or sagas, because extension methods are available only on the message session. The differences in the API are covered in more detail in [Callbacks](/nservicebus/messaging/callbacks.md).

The `NServiceBus.Callbacks` package has to be referenced only by the requesting endpoint. The responding endpoint needs `NServiceBus.Callbacks` only if it replies  with `int` or `enum` types.

snippet: 5to6-Callbacks

In order to use callbacks, the endpoint must be uniquely addressable:

snippet: 5to6-Callbacks-InstanceId

NOTE: This ID should never be hard-coded; it can be read from a configuration file or from the environment (e.g. role ID in Azure) so that it can be changed without code changes and redeployment.
