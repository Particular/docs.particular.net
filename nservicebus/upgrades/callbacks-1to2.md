---
title: Upgrade Version 1 to 2
summary: Instructions on how to upgrade NServiceBus.Callbacks Version 1 to 2.
component: Callbacks
reviewed: 2020-12-22
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Enable callbacks

Callbacks have been made opt-in. In order to make callbacks work they have to be enabled explicitly. 

snippet: 1to2-Callbacks-InstanceId-Default

Enabling callbacks in default mode requires the endpoint to be made [uniquely addressable](/nservicebus/messaging/callbacks.md#message-routing).

## Enable callbacks without requests

Endpoints only replying to callbacks with object messages like the following:

snippet: 1to2-Callbacks-ObjectCallbackResponse

don't require a reference to the callbacks package. 

For endpoints replying with int or enum results, the callbacks package can be enabled in response-only mode:

snippet: 1to2-Callbacks-InstanceId-NoRequests
